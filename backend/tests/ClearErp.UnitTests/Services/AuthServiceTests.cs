using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;
using ClearErp.Infrastructure.Auth;
using ClearErp.Infrastructure.Persistence;
using ClearErp.Infrastructure.Persistence.Repositories;
using ClearErp.Infrastructure.Persistence.Seeding;

namespace ClearErp.UnitTests.Services;

public sealed class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_Should_ReturnTokenRoles_AndCreateSuccessAuditLog()
    {
        await using var dbContext = CreateDbContext();
        var passwordHasher = new Pbkdf2PasswordHasher();
        var adminRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };

        var user = new User
        {
            Id = SeedConstants.AdminUserId,
            Email = "admin@clearerp.local",
            PasswordHash = passwordHasher.HashPassword("Admin123!"),
            FullName = "Admin",
            IsActive = true
        };
        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = adminRole.Id,
            Role = adminRole
        });

        dbContext.Roles.Add(adminRole);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var authService = new AuthService(
            new UserRepository(dbContext),
            passwordHasher,
            new JwtTokenGenerator(Options.Create(new JwtOptions
            {
                Issuer = "ClearErp",
                Audience = "ClearErp.Client",
                Key = "local-development-jwt-signing-key-change-before-production",
                ExpirationMinutes = 60
            })),
            new AuditLogRepository(dbContext),
            dbContext);

        var result = await authService.LoginAsync("admin@clearerp.local", "Admin123!");

        var auditLog = await dbContext.AuditLogs.SingleAsync();
        Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
        Assert.Equal("admin@clearerp.local", result.Email);
        Assert.Contains("Admin", result.Roles);
        Assert.Equal("LoginSucceeded", auditLog.Action);
    }

    [Fact]
    public async Task LoginAsync_Should_ThrowUnauthorized_AndCreateFailureAuditLog_WhenPasswordIsInvalid()
    {
        await using var dbContext = CreateDbContext();
        var passwordHasher = new Pbkdf2PasswordHasher();

        dbContext.Users.Add(new User
        {
            Id = SeedConstants.AdminUserId,
            Email = "admin@clearerp.local",
            PasswordHash = passwordHasher.HashPassword("Admin123!"),
            FullName = "Admin",
            IsActive = true
        });
        await dbContext.SaveChangesAsync();

        var authService = new AuthService(
            new UserRepository(dbContext),
            passwordHasher,
            new JwtTokenGenerator(Options.Create(new JwtOptions
            {
                Issuer = "ClearErp",
                Audience = "ClearErp.Client",
                Key = "local-development-jwt-signing-key-change-before-production",
                ExpirationMinutes = 60
            })),
            new AuditLogRepository(dbContext),
            dbContext);

        var exception = await Assert.ThrowsAsync<UnauthorizedException>(() =>
            authService.LoginAsync("admin@clearerp.local", "WrongPassword!"));

        var auditLog = await dbContext.AuditLogs.SingleAsync();

        Assert.Equal("Invalid email or password.", exception.Message);
        Assert.Equal("LoginFailed", auditLog.Action);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}

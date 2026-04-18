using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Application.Common.Interfaces.Security;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;

namespace ClearErp.Infrastructure.Auth;

public sealed class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IAuditLogRepository auditLogRepository,
    IApplicationDbContext dbContext) : IAuthService
{
    public async Task<AuthResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();
        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null || !user.IsActive || !passwordHasher.VerifyPassword(user.PasswordHash, password))
        {
            if (user is not null)
            {
                await auditLogRepository.AddAsync(
                    AuditLog.Create("LoginFailed", nameof(User), user.Id, $"Failed login attempt for {normalizedEmail}", user.Id),
                    cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            throw new UnauthorizedException("Invalid email or password.");
        }

        var roles = user.UserRoles
            .Select(x => x.Role?.Name)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Cast<string>()
            .ToArray();

        var (accessToken, expiresAtUtc) = jwtTokenGenerator.GenerateToken(user, roles);

        await auditLogRepository.AddAsync(
            AuditLog.Create("LoginSucceeded", nameof(User), user.Id, $"Successful login for {user.Email}", user.Id),
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new AuthResult(
            accessToken,
            expiresAtUtc,
            user.Id,
            user.Email,
            user.FullName,
            roles);
    }
}

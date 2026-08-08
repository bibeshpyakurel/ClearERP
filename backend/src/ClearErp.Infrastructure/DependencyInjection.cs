using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Application.Common.Interfaces.Security;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Infrastructure.Auth;
using ClearErp.Infrastructure.BackgroundJobs;
using ClearErp.Infrastructure.Persistence;
using ClearErp.Infrastructure.Persistence.Repositories;
using ClearErp.Infrastructure.Services;

namespace ClearErp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = ResolveConnectionString(configuration);

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

        services.AddScoped<ITenantContext, TenantContext>();
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IInventoryBalanceRepository, InventoryBalanceRepository>();
        services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGoodsReceiptService, GoodsReceiptService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
        services.AddScoped<IReportingService, ReportingService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IDemoResetService, DemoResetService>();

        services.AddHostedService<LowStockMonitorService>();

        return services;
    }

    private static string ResolveConnectionString(IConfiguration configuration)
    {
        // DATABASE_URL takes priority (Railway, Heroku, Render inject this as a postgres:// URI)
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        if (!string.IsNullOrWhiteSpace(databaseUrl))
        {
            var uri = new Uri(databaseUrl);
            var userInfo = uri.UserInfo.Split(':', 2);

            // Uri.Port is -1 when the URI omits the port, which managed providers
            // such as Neon do by default.
            var port = uri.Port > 0 ? uri.Port : 5432;

            // Userinfo is percent-encoded per RFC 3986, and generated passwords
            // routinely contain characters that must be decoded before Npgsql
            // sees them.
            var username = Uri.UnescapeDataString(userInfo[0]);
            var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;

            return $"Host={uri.Host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true;";
        }

        return configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Database connection string is not configured. Set DATABASE_URL or ConnectionStrings__DefaultConnection.");
    }
}

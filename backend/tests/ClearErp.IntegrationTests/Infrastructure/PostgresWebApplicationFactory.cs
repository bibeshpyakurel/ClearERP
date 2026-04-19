using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ClearErp.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace ClearErp.IntegrationTests.Infrastructure;

/// <summary>
/// Spins up a real PostgreSQL container for each test class that uses it.
/// In CI, uses the service container; locally uses Testcontainers.
/// </summary>
public sealed class PostgresWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private PostgreSqlContainer? _postgres;
    private string _connectionString = string.Empty;
    private readonly bool _useTestcontainers;

    public PostgresWebApplicationFactory()
    {
        // Use CI service container if available, otherwise use Testcontainers
        var ciConnectionString = Environment.GetEnvironmentVariable("CI_POSTGRES_CONNECTION_STRING");
        if (!string.IsNullOrEmpty(ciConnectionString))
        {
            _connectionString = ciConnectionString;
            _useTestcontainers = false;
        }
        else
        {
            _useTestcontainers = true;
        }
    }

    public async Task InitializeAsync()
    {
        if (_useTestcontainers)
        {
            _postgres = new PostgreSqlBuilder()
                .WithImage("postgres:16-alpine")
                .WithDatabase($"clear_erp_test_{Guid.NewGuid():N}")
                .WithUsername("postgres")
                .WithPassword("postgres_test")
                .Build();

            await _postgres.StartAsync();
            _connectionString = _postgres.GetConnectionString();
        }
    }

    public new async Task DisposeAsync()
    {
        if (_postgres is not null)
        {
            await _postgres.StopAsync();
        }
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Replace the production DbContext registration with one that points at the test database.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_connectionString));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Run migrations after DI is configured with the test database
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();

        return host;
    }
}

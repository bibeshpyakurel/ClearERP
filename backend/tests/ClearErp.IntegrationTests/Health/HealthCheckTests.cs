using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ClearErp.IntegrationTests.Infrastructure;

namespace ClearErp.IntegrationTests.Health;

/// <summary>
/// Verifies the /health endpoint against a real PostgreSQL container.
/// This is an end-to-end proof that migrations run, the DB check executes,
/// and the application reports Healthy.
/// </summary>
public sealed class HealthCheckTests(PostgresWebApplicationFactory factory)
    : IClassFixture<PostgresWebApplicationFactory>
{
    [Fact]
    public async Task Health_WithRunningDatabase_Returns200Healthy()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal("Healthy", root.GetProperty("status").GetString());

        var checks = root.GetProperty("checks").EnumerateArray().ToList();
        var dbCheck = checks.FirstOrDefault(c => c.GetProperty("name").GetString() == "postgres");

        Assert.True(dbCheck.ValueKind != JsonValueKind.Undefined, "Expected 'postgres' health check entry.");
        Assert.Equal("Healthy", dbCheck.GetProperty("status").GetString());
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsTokenAndUserId()
    {
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            Email = "admin@clearfurniture.local",
            Password = "Admin123!",
            TenantSlug = "furniture"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.True(root.TryGetProperty("accessToken", out var token));
        Assert.False(string.IsNullOrWhiteSpace(token.GetString()), "Expected a non-empty access token.");

        Assert.True(root.TryGetProperty("userId", out var userId));
        Assert.True(Guid.TryParse(userId.GetString(), out _), "Expected a valid GUID userId.");
    }
}

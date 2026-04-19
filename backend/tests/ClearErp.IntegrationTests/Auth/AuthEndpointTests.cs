using System.Net;
using System.Net.Http.Json;
using ClearErp.Api.Middleware;
using ClearErp.IntegrationTests.Infrastructure;

namespace ClearErp.IntegrationTests.Auth;

public sealed class AuthEndpointTests : IClassFixture<PostgresWebApplicationFactory>
{
    private readonly PostgresWebApplicationFactory _factory;

    public AuthEndpointTests(PostgresWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Me_WithoutAuthentication_Should_ReturnUnauthorizedErrorPayload()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        Assert.NotNull(payload);
        Assert.Equal(401, payload.Status);
        Assert.Equal("Unauthorized", payload.Title);
    }

    [Fact]
    public async Task Login_WithMissingPassword_Should_ReturnValidationErrorPayload()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            Email = "admin@clearfurniture.local",
            Password = string.Empty,
            TenantSlug = "furniture"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        Assert.NotNull(payload);
        Assert.Equal(400, payload.Status);
        Assert.Equal("Validation failed", payload.Title);
        Assert.NotNull(payload.Errors);
        Assert.Contains("Password", payload.Errors.Keys);
    }
}

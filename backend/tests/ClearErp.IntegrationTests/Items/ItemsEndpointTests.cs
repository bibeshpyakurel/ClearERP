using System.Net;
using System.Net.Http.Json;
using ClearErp.Api.Middleware;
using ClearErp.IntegrationTests.Infrastructure;

namespace ClearErp.IntegrationTests.Items;

public sealed class ItemsEndpointTests : IClassFixture<PostgresWebApplicationFactory>
{
    private readonly PostgresWebApplicationFactory _factory;

    public ItemsEndpointTests(PostgresWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetItems_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/items");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        Assert.NotNull(payload);
        Assert.Equal(401, payload.Status);
        Assert.Equal("Unauthorized", payload.Title);
    }

    [Fact]
    public async Task GetItemById_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"/api/items/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidPayload_Should_ReturnValidationErrorResponse()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            Email = string.Empty,
            Password = "123"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        Assert.NotNull(payload);
        Assert.Equal(400, payload.Status);
        Assert.Equal("Validation failed", payload.Title);
        Assert.NotNull(payload.Errors);
        Assert.Contains("Email", payload.Errors.Keys);
        Assert.Contains("Password", payload.Errors.Keys);
    }
}

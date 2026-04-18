using System.Net;
using ClearErp.IntegrationTests.Infrastructure;

namespace ClearErp.IntegrationTests.Inventory;

public sealed class InventoryEndpointTests : IClassFixture<PostgresWebApplicationFactory>
{
    private readonly PostgresWebApplicationFactory _factory;

    public InventoryEndpointTests(PostgresWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetBalances_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/inventory/balances");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetTransactions_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/inventory/transactions");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostIssue_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/api/inventory/issues", content: null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostAdjustment_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/api/inventory/adjustments", content: null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

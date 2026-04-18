using System.Net;
using ClearErp.IntegrationTests.Infrastructure;

namespace ClearErp.IntegrationTests.PurchaseOrders;

public sealed class PurchaseOrdersEndpointTests : IClassFixture<PostgresWebApplicationFactory>
{
    private readonly PostgresWebApplicationFactory _factory;

    public PurchaseOrdersEndpointTests(PostgresWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetPurchaseOrders_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/purchase-orders");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetPurchaseOrderById_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"/api/purchase-orders/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

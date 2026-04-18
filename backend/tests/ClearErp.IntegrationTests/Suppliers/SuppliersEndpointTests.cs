using System.Net;
using ClearErp.IntegrationTests.Infrastructure;

namespace ClearErp.IntegrationTests.Suppliers;

public sealed class SuppliersEndpointTests : IClassFixture<PostgresWebApplicationFactory>
{
    private readonly PostgresWebApplicationFactory _factory;

    public SuppliersEndpointTests(PostgresWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetSuppliers_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/suppliers");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSupplierById_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"/api/suppliers/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

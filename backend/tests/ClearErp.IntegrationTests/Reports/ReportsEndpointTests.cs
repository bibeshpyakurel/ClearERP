using System.Net;
using ClearErp.IntegrationTests.Infrastructure;

namespace ClearErp.IntegrationTests.Reports;

public sealed class ReportsEndpointTests : IClassFixture<PostgresWebApplicationFactory>
{
    private readonly PostgresWebApplicationFactory _factory;

    public ReportsEndpointTests(PostgresWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetStockSummary_WithoutAuthentication_Should_ReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/reports/stock-summary");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

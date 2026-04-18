namespace ClearErp.Api.Contracts.Items;

public sealed record ItemSummaryResponse(
    Guid Id,
    string Sku,
    string Name,
    string Unit,
    int ReorderLevel,
    decimal StandardCost,
    bool IsActive);

namespace ClearErp.Api.Contracts.Items;

public sealed record ItemResponse(
    Guid Id,
    Guid CategoryId,
    string CategoryName,
    string Sku,
    string Name,
    string? Description,
    string Unit,
    int ReorderLevel,
    decimal StandardCost,
    bool IsActive);

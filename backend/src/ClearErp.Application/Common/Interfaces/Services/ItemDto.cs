namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record ItemDto(
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

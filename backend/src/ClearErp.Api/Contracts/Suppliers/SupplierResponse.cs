namespace ClearErp.Api.Contracts.Suppliers;

public sealed record SupplierResponse(
    Guid Id,
    string Name,
    string ContactName,
    string Email,
    string Phone,
    string? Notes,
    bool IsActive,
    IReadOnlyCollection<SupplierItemResponse> Items);

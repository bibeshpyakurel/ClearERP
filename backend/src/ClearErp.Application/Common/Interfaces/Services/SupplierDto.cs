namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record SupplierDto(
    Guid Id,
    string Name,
    string ContactName,
    string Email,
    string Phone,
    string? Notes,
    bool IsActive,
    IReadOnlyCollection<SupplierItemDto> Items);

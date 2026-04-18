namespace ClearErp.Application.Common.Interfaces.Services;

public interface ISupplierService
{
    Task<IReadOnlyList<SupplierDto>> SearchSuppliersAsync(SupplierFilter filter, CancellationToken cancellationToken = default);
    Task<SupplierDto?> GetSupplierByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SupplierDto> CreateSupplierAsync(
        string name,
        string contactName,
        string email,
        string phone,
        string? notes,
        IReadOnlyCollection<UpsertSupplierItemRequest> items,
        CancellationToken cancellationToken = default);
    Task<SupplierDto> UpdateSupplierAsync(
        Guid id,
        string name,
        string contactName,
        string email,
        string phone,
        string? notes,
        IReadOnlyCollection<UpsertSupplierItemRequest> items,
        CancellationToken cancellationToken = default);
    Task<SupplierDto> SetSupplierStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default);
}

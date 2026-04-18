namespace ClearErp.Application.Common.Interfaces.Services;

public interface IPurchaseOrderService
{
    Task<IReadOnlyList<PurchaseOrderDto>> SearchPurchaseOrdersAsync(
        PurchaseOrderFilter filter,
        CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> CreatePurchaseOrderAsync(
        string poNumber,
        Guid supplierId,
        Guid createdByUserId,
        DateTime orderDate,
        DateTime? expectedDate,
        IReadOnlyCollection<UpsertPurchaseOrderLineRequest> lines,
        CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> UpdatePurchaseOrderAsync(
        Guid id,
        Guid supplierId,
        DateTime orderDate,
        DateTime? expectedDate,
        IReadOnlyCollection<UpsertPurchaseOrderLineRequest> lines,
        CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> ApprovePurchaseOrderAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default);
    Task<PurchaseOrderDto> CancelPurchaseOrderAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default);
}

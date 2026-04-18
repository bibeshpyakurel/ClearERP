namespace ClearErp.Api.Contracts.PurchaseOrders;

public sealed record PurchaseOrderResponse(
    Guid Id,
    string PoNumber,
    Guid SupplierId,
    string SupplierName,
    string Status,
    DateTime OrderDate,
    DateTime? ExpectedDate,
    Guid CreatedByUserId,
    decimal TotalAmount,
    IReadOnlyCollection<PurchaseOrderLineResponse> Lines);

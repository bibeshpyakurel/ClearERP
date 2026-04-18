namespace ClearErp.Api.Contracts.GoodsReceipts;

public sealed record GoodsReceiptResponse(
    Guid Id,
    Guid PurchaseOrderId,
    string ReceiptNumber,
    DateTime ReceivedAt,
    Guid ReceivedByUserId,
    decimal TotalReceivedAmount,
    IReadOnlyCollection<GoodsReceiptLineResponse> Lines);

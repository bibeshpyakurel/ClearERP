namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record GoodsReceiptDto(
    Guid Id,
    Guid PurchaseOrderId,
    string ReceiptNumber,
    DateTime ReceivedAt,
    Guid ReceivedByUserId,
    decimal TotalReceivedAmount,
    IReadOnlyCollection<GoodsReceiptLineDto> Lines);

namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record GoodsReceiptLineDto(
    Guid Id,
    Guid PurchaseOrderLineId,
    Guid ItemId,
    string ItemSku,
    string ItemName,
    int ReceivedQuantity);

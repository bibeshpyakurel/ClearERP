namespace ClearErp.Api.Contracts.GoodsReceipts;

public sealed record GoodsReceiptLineResponse(
    Guid Id,
    Guid PurchaseOrderLineId,
    Guid ItemId,
    string ItemSku,
    string ItemName,
    int ReceivedQuantity);

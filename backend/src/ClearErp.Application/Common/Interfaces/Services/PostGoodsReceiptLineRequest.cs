namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record PostGoodsReceiptLineRequest(
    Guid PurchaseOrderLineId,
    Guid ItemId,
    int ReceivedQuantity,
    Guid WarehouseId,
    Guid LocationId);

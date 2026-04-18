using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.GoodsReceipts;

/// <summary>
/// Represents one receipt line tied to a purchase order line.
/// </summary>
public sealed record PostGoodsReceiptLineRequest(
    [property: Required] Guid PurchaseOrderLineId,
    [property: Required] Guid ItemId,
    [property: Range(1, int.MaxValue)] int ReceivedQuantity,
    [property: Required] Guid WarehouseId,
    [property: Required] Guid LocationId);

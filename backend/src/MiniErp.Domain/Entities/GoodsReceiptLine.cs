using MiniErp.Domain.Common;

namespace MiniErp.Domain.Entities;

public sealed class GoodsReceiptLine : BaseEntity
{
    public Guid GoodsReceiptId { get; set; }
    public Guid PurchaseOrderLineId { get; set; }
    public Guid ItemId { get; set; }
    public int ReceivedQuantity { get; set; }

    public GoodsReceipt? GoodsReceipt { get; set; }
    public PurchaseOrderLine? PurchaseOrderLine { get; set; }
    public Item? Item { get; set; }
}

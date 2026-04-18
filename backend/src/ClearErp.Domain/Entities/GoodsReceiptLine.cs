using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class GoodsReceiptLine : BaseEntity
{
    public Guid GoodsReceiptId { get; set; }
    public Guid PurchaseOrderLineId { get; set; }
    public Guid ItemId { get; set; }
    public int ReceivedQuantity { get; set; }

    public GoodsReceipt? GoodsReceipt { get; set; }
    public PurchaseOrderLine? PurchaseOrderLine { get; set; }
    public Item? Item { get; set; }

    public static GoodsReceiptLine Create(Guid goodsReceiptId, Guid purchaseOrderLineId, Guid itemId, int receivedQuantity)
    {
        Guard.AgainstEmpty(goodsReceiptId, nameof(goodsReceiptId));
        Guard.AgainstEmpty(purchaseOrderLineId, nameof(purchaseOrderLineId));
        Guard.AgainstEmpty(itemId, nameof(itemId));
        Guard.AgainstZeroOrNegative(receivedQuantity, nameof(receivedQuantity));

        return new GoodsReceiptLine
        {
            GoodsReceiptId = goodsReceiptId,
            PurchaseOrderLineId = purchaseOrderLineId,
            ItemId = itemId,
            ReceivedQuantity = receivedQuantity
        };
    }
}

using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class GoodsReceipt : BaseEntity
{
    public Guid PurchaseOrderId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public Guid ReceivedByUserId { get; set; }

    public PurchaseOrder? PurchaseOrder { get; set; }
    public User? ReceivedByUser { get; set; }
    public ICollection<GoodsReceiptLine> Lines { get; set; } = new List<GoodsReceiptLine>();

    public static GoodsReceipt Create(Guid purchaseOrderId, string receiptNumber, Guid receivedByUserId, DateTime? receivedAt = null)
    {
        Guard.AgainstEmpty(purchaseOrderId, nameof(purchaseOrderId));
        Guard.AgainstNullOrWhiteSpace(receiptNumber, nameof(receiptNumber), 50);
        Guard.AgainstEmpty(receivedByUserId, nameof(receivedByUserId));

        return new GoodsReceipt
        {
            PurchaseOrderId = purchaseOrderId,
            ReceiptNumber = receiptNumber.Trim().ToUpperInvariant(),
            ReceivedByUserId = receivedByUserId,
            ReceivedAt = receivedAt ?? DateTime.UtcNow
        };
    }

    public void AddLine(Guid purchaseOrderLineId, Guid itemId, int receivedQuantity)
    {
        Lines.Add(GoodsReceiptLine.Create(Id, purchaseOrderLineId, itemId, receivedQuantity));
        Touch();
    }
}

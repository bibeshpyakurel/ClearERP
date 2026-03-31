using MiniErp.Domain.Common;

namespace MiniErp.Domain.Entities;

public sealed class GoodsReceipt : BaseEntity
{
    public Guid PurchaseOrderId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public Guid ReceivedByUserId { get; set; }

    public PurchaseOrder? PurchaseOrder { get; set; }
    public User? ReceivedByUser { get; set; }
    public ICollection<GoodsReceiptLine> Lines { get; set; } = new List<GoodsReceiptLine>();
}

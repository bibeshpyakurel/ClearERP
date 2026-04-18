using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class PurchaseOrderLine : BaseEntity
{
    public Guid PurchaseOrderId { get; set; }
    public Guid ItemId { get; set; }
    public int OrderedQuantity { get; set; }
    public int ReceivedQuantity { get; set; }
    public decimal UnitCost { get; set; }

    public PurchaseOrder? PurchaseOrder { get; set; }
    public Item? Item { get; set; }
    public ICollection<GoodsReceiptLine> GoodsReceiptLines { get; set; } = new List<GoodsReceiptLine>();

    public bool IsFullyReceived => ReceivedQuantity >= OrderedQuantity;
    public decimal LineTotal => OrderedQuantity * UnitCost;

    public static PurchaseOrderLine Create(Guid purchaseOrderId, Guid itemId, int orderedQuantity, decimal unitCost)
    {
        Guard.AgainstEmpty(purchaseOrderId, nameof(purchaseOrderId));
        Guard.AgainstEmpty(itemId, nameof(itemId));
        Guard.AgainstZeroOrNegative(orderedQuantity, nameof(orderedQuantity));
        Guard.AgainstNegative(unitCost, nameof(unitCost));

        return new PurchaseOrderLine
        {
            PurchaseOrderId = purchaseOrderId,
            ItemId = itemId,
            OrderedQuantity = orderedQuantity,
            ReceivedQuantity = 0,
            UnitCost = unitCost
        };
    }

    public void RegisterReceipt(int receivedQuantity)
    {
        Guard.AgainstZeroOrNegative(receivedQuantity, nameof(receivedQuantity));

        if (ReceivedQuantity + receivedQuantity > OrderedQuantity)
        {
            throw new DomainException("Received quantity cannot exceed ordered quantity.");
        }

        ReceivedQuantity += receivedQuantity;
        Touch();
    }

    public void UpdateDetails(int orderedQuantity, decimal unitCost)
    {
        Guard.AgainstZeroOrNegative(orderedQuantity, nameof(orderedQuantity));
        Guard.AgainstNegative(unitCost, nameof(unitCost));

        if (ReceivedQuantity > orderedQuantity)
        {
            throw new DomainException("Ordered quantity cannot be reduced below the quantity already received.");
        }

        OrderedQuantity = orderedQuantity;
        UnitCost = unitCost;
        Touch();
    }
}

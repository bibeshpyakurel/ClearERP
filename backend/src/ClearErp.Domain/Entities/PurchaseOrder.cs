using ClearErp.Domain.Common;
using ClearErp.Domain.Enums;

namespace ClearErp.Domain.Entities;

public sealed class PurchaseOrder : BaseEntity
{
    public string PoNumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpectedDate { get; set; }
    public Guid CreatedByUserId { get; set; }

    public Supplier? Supplier { get; set; }
    public User? CreatedByUser { get; set; }
    public ICollection<PurchaseOrderLine> Lines { get; set; } = new List<PurchaseOrderLine>();
    public ICollection<GoodsReceipt> GoodsReceipts { get; set; } = new List<GoodsReceipt>();
    public decimal TotalAmount => Lines.Sum(x => x.LineTotal);

    public static PurchaseOrder Create(string poNumber, Guid supplierId, Guid createdByUserId, DateTime orderDate, DateTime? expectedDate = null)
    {
        Guard.AgainstNullOrWhiteSpace(poNumber, nameof(poNumber), 50);
        Guard.AgainstEmpty(supplierId, nameof(supplierId));
        Guard.AgainstEmpty(createdByUserId, nameof(createdByUserId));

        if (expectedDate.HasValue && expectedDate.Value < orderDate)
        {
            throw new DomainException("Expected date cannot be earlier than the order date.");
        }

        return new PurchaseOrder
        {
            PoNumber = poNumber.Trim().ToUpperInvariant(),
            SupplierId = supplierId,
            CreatedByUserId = createdByUserId,
            OrderDate = orderDate,
            ExpectedDate = expectedDate,
            Status = PurchaseOrderStatus.Draft
        };
    }

    public void AddLine(Guid itemId, int orderedQuantity, decimal unitCost)
    {
        if (Status is not PurchaseOrderStatus.Draft)
        {
            throw new DomainException("Lines can only be added while the purchase order is in draft status.");
        }

        if (Lines.Any(x => x.ItemId == itemId))
        {
            throw new DomainException("Each item may only appear once on a purchase order.");
        }

        Lines.Add(PurchaseOrderLine.Create(Id, itemId, orderedQuantity, unitCost));
        Touch();
    }

    public void UpdateDraftDetails(Guid supplierId, DateTime orderDate, DateTime? expectedDate)
    {
        if (Status is not PurchaseOrderStatus.Draft)
        {
            throw new DomainException("Only draft purchase orders can be edited.");
        }

        Guard.AgainstEmpty(supplierId, nameof(supplierId));

        if (expectedDate.HasValue && expectedDate.Value < orderDate)
        {
            throw new DomainException("Expected date cannot be earlier than the order date.");
        }

        SupplierId = supplierId;
        OrderDate = orderDate;
        ExpectedDate = expectedDate;
        Touch();
    }

    public void ReplaceLines(IEnumerable<(Guid ItemId, int OrderedQuantity, decimal UnitCost)> lines)
    {
        if (Status is not PurchaseOrderStatus.Draft)
        {
            throw new DomainException("Only draft purchase orders can have their lines replaced.");
        }

        var materializedLines = lines.ToList();
        if (materializedLines.Count == 0)
        {
            throw new DomainException("A purchase order must have at least one line.");
        }

        if (materializedLines.Select(x => x.ItemId).Distinct().Count() != materializedLines.Count)
        {
            throw new DomainException("Each item may only appear once on a purchase order.");
        }

        var incomingByItemId = materializedLines.ToDictionary(x => x.ItemId);
        var existingLines = Lines.ToList();

        foreach (var existingLine in existingLines)
        {
            if (!incomingByItemId.ContainsKey(existingLine.ItemId))
            {
                Lines.Remove(existingLine);
            }
        }

        foreach (var line in materializedLines)
        {
            var existingLine = Lines.SingleOrDefault(x => x.ItemId == line.ItemId);

            if (existingLine is not null)
            {
                existingLine.UpdateDetails(line.OrderedQuantity, line.UnitCost);
                continue;
            }

            Lines.Add(PurchaseOrderLine.Create(Id, line.ItemId, line.OrderedQuantity, line.UnitCost));
        }

        Touch();
    }

    public void Approve()
    {
        if (Status is not PurchaseOrderStatus.Draft)
        {
            throw new DomainException("Only draft purchase orders can be approved.");
        }

        if (Lines.Count == 0)
        {
            throw new DomainException("A purchase order must have at least one line before approval.");
        }

        Status = PurchaseOrderStatus.Approved;
        Touch();
    }

    public void Cancel()
    {
        if (Status is PurchaseOrderStatus.Completed)
        {
            throw new DomainException("Completed purchase orders cannot be cancelled.");
        }

        if (Status is PurchaseOrderStatus.Cancelled)
        {
            throw new DomainException("Purchase order is already cancelled.");
        }

        Status = PurchaseOrderStatus.Cancelled;
        Touch();
    }

    public void RegisterReceipt(Guid purchaseOrderLineId, int receivedQuantity)
    {
        if (Status is PurchaseOrderStatus.Cancelled or PurchaseOrderStatus.Completed)
        {
            throw new DomainException("This purchase order cannot receive additional stock.");
        }

        var line = Lines.SingleOrDefault(x => x.Id == purchaseOrderLineId);
        if (line is null)
        {
            throw new DomainException("Purchase order line was not found.");
        }

        line.RegisterReceipt(receivedQuantity);

        Status = Lines.All(x => x.IsFullyReceived)
            ? PurchaseOrderStatus.Completed
            : PurchaseOrderStatus.PartiallyReceived;

        Touch();
    }
}

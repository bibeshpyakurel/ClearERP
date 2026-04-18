using ClearErp.Domain.Common;
using ClearErp.Domain.Enums;

namespace ClearErp.Domain.Entities;

public sealed class InventoryTransaction : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid LocationId { get; set; }
    public InventoryTransactionType TransactionType { get; set; }
    public string ReferenceType { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    public string? Reason { get; set; }
    public int QuantityChange { get; set; }
    public int BalanceAfter { get; set; }
    public Guid PerformedByUserId { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    public Item? Item { get; set; }
    public Warehouse? Warehouse { get; set; }
    public Location? Location { get; set; }
    public User? PerformedByUser { get; set; }

    public static InventoryTransaction Create(
        Guid itemId,
        Guid warehouseId,
        Guid locationId,
        InventoryTransactionType transactionType,
        string referenceType,
        string? reason,
        int quantityChange,
        int balanceAfter,
        Guid performedByUserId,
        Guid? referenceId = null)
    {
        Guard.AgainstEmpty(itemId, nameof(itemId));
        Guard.AgainstEmpty(warehouseId, nameof(warehouseId));
        Guard.AgainstEmpty(locationId, nameof(locationId));
        Guard.AgainstEmpty(performedByUserId, nameof(performedByUserId));
        Guard.AgainstNullOrWhiteSpace(referenceType, nameof(referenceType), 50);

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Guard.AgainstNullOrWhiteSpace(reason, nameof(reason), 500);
        }

        if (quantityChange == 0)
        {
            throw new DomainException("Quantity change must not be zero.");
        }

        Guard.AgainstNegative(balanceAfter, nameof(balanceAfter));

        return new InventoryTransaction
        {
            ItemId = itemId,
            WarehouseId = warehouseId,
            LocationId = locationId,
            TransactionType = transactionType,
            ReferenceType = referenceType.Trim(),
            ReferenceId = referenceId,
            Reason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim(),
            QuantityChange = quantityChange,
            BalanceAfter = balanceAfter,
            PerformedByUserId = performedByUserId,
            PerformedAt = DateTime.UtcNow
        };
    }
}

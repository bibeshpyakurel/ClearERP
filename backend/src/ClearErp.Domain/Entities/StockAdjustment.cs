using ClearErp.Domain.Common;
using ClearErp.Domain.Enums;

namespace ClearErp.Domain.Entities;

public sealed class StockAdjustment : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid LocationId { get; set; }
    public AdjustmentType AdjustmentType { get; set; }
    public int QuantityDelta { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Guid PerformedByUserId { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    public Item? Item { get; set; }
    public Warehouse? Warehouse { get; set; }
    public Location? Location { get; set; }
    public User? PerformedByUser { get; set; }

    public static StockAdjustment Create(
        Guid itemId,
        Guid warehouseId,
        Guid locationId,
        AdjustmentType adjustmentType,
        int quantityDelta,
        string reason,
        Guid performedByUserId)
    {
        Guard.AgainstEmpty(itemId, nameof(itemId));
        Guard.AgainstEmpty(warehouseId, nameof(warehouseId));
        Guard.AgainstEmpty(locationId, nameof(locationId));
        Guard.AgainstEmpty(performedByUserId, nameof(performedByUserId));
        Guard.AgainstZeroOrNegative(quantityDelta, nameof(quantityDelta));
        Guard.AgainstNullOrWhiteSpace(reason, nameof(reason), 500);

        return new StockAdjustment
        {
            ItemId = itemId,
            WarehouseId = warehouseId,
            LocationId = locationId,
            AdjustmentType = adjustmentType,
            QuantityDelta = quantityDelta,
            Reason = reason.Trim(),
            PerformedByUserId = performedByUserId,
            PerformedAt = DateTime.UtcNow
        };
    }
}

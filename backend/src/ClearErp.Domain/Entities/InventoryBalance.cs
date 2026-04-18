using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class InventoryBalance : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid LocationId { get; set; }
    public int QuantityOnHand { get; set; }
    public int QuantityReserved { get; set; }
    public int QuantityAvailable { get; set; }
    public uint RowVersion { get; set; }

    public Item? Item { get; set; }
    public Warehouse? Warehouse { get; set; }
    public Location? Location { get; set; }

    public static InventoryBalance Create(Guid itemId, Guid warehouseId, Guid locationId, int quantityOnHand = 0, int quantityReserved = 0)
    {
        Guard.AgainstEmpty(itemId, nameof(itemId));
        Guard.AgainstEmpty(warehouseId, nameof(warehouseId));
        Guard.AgainstEmpty(locationId, nameof(locationId));
        Guard.AgainstNegative(quantityOnHand, nameof(quantityOnHand));
        Guard.AgainstNegative(quantityReserved, nameof(quantityReserved));

        if (quantityReserved > quantityOnHand)
        {
            throw new DomainException("Reserved quantity cannot exceed quantity on hand.");
        }

        return new InventoryBalance
        {
            ItemId = itemId,
            WarehouseId = warehouseId,
            LocationId = locationId,
            QuantityOnHand = quantityOnHand,
            QuantityReserved = quantityReserved,
            QuantityAvailable = quantityOnHand - quantityReserved
        };
    }

    public void Receive(int quantity)
    {
        Guard.AgainstZeroOrNegative(quantity, nameof(quantity));
        QuantityOnHand += quantity;
        QuantityAvailable = QuantityOnHand - QuantityReserved;
        Touch();
    }

    public void Issue(int quantity)
    {
        Guard.AgainstZeroOrNegative(quantity, nameof(quantity));

        if (quantity > QuantityAvailable)
        {
            throw new DomainException("Cannot issue more inventory than is available.");
        }

        QuantityOnHand -= quantity;
        QuantityAvailable = QuantityOnHand - QuantityReserved;
        Touch();
    }

    public void Adjust(int quantityDelta)
    {
        if (QuantityOnHand + quantityDelta < 0)
        {
            throw new DomainException("Inventory adjustment would make quantity on hand negative.");
        }

        QuantityOnHand += quantityDelta;

        if (QuantityReserved > QuantityOnHand)
        {
            throw new DomainException("Reserved quantity cannot exceed quantity on hand after adjustment.");
        }

        QuantityAvailable = QuantityOnHand - QuantityReserved;
        Touch();
    }
}

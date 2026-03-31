using MiniErp.Domain.Common;

namespace MiniErp.Domain.Entities;

public sealed class InventoryBalance : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid LocationId { get; set; }
    public int QuantityOnHand { get; set; }
    public int QuantityReserved { get; set; }
    public int QuantityAvailable { get; set; }

    public Item? Item { get; set; }
    public Warehouse? Warehouse { get; set; }
    public Location? Location { get; set; }
}

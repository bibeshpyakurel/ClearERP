using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class Location : BaseEntity
{
    public Guid WarehouseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public Warehouse? Warehouse { get; set; }
    public ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();
    public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
    public ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();

    public static Location Create(Guid warehouseId, string name, string code)
    {
        Guard.AgainstEmpty(warehouseId, nameof(warehouseId));
        Guard.AgainstNullOrWhiteSpace(name, nameof(name), 100);
        Guard.AgainstNullOrWhiteSpace(code, nameof(code), 20);

        return new Location
        {
            WarehouseId = warehouseId,
            Name = name.Trim(),
            Code = code.Trim().ToUpperInvariant()
        };
    }
}

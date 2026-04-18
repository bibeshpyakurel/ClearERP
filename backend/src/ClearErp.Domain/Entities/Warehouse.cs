using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class Warehouse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public ICollection<Location> Locations { get; set; } = new List<Location>();
    public ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();
    public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
    public ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();

    public static Warehouse Create(string name, string code)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name), 100);
        Guard.AgainstNullOrWhiteSpace(code, nameof(code), 20);

        return new Warehouse
        {
            Name = name.Trim(),
            Code = code.Trim().ToUpperInvariant()
        };
    }
}

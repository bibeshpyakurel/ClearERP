using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class Item : BaseEntity
{
    public Guid CategoryId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Unit { get; set; } = string.Empty;
    public int ReorderLevel { get; set; }
    public decimal StandardCost { get; set; }
    public bool IsActive { get; set; } = true;

    public Category? Category { get; set; }
    public ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();
    public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
    public ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();
    public ICollection<GoodsReceiptLine> GoodsReceiptLines { get; set; } = new List<GoodsReceiptLine>();
    public ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();
    public ICollection<SupplierItem> SupplierItems { get; set; } = new List<SupplierItem>();

    public static Item Create(
        Guid categoryId,
        string sku,
        string name,
        string unit,
        int reorderLevel,
        decimal standardCost,
        string? description = null)
    {
        Guard.AgainstEmpty(categoryId, nameof(categoryId));
        Guard.AgainstNullOrWhiteSpace(sku, nameof(sku), 64);
        Guard.AgainstNullOrWhiteSpace(name, nameof(name), 200);
        Guard.AgainstNullOrWhiteSpace(unit, nameof(unit), 32);
        Guard.AgainstNegative(reorderLevel, nameof(reorderLevel));
        Guard.AgainstNegative(standardCost, nameof(standardCost));

        return new Item
        {
            CategoryId = categoryId,
            Sku = sku.Trim().ToUpperInvariant(),
            Name = name.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Unit = unit.Trim().ToUpperInvariant(),
            ReorderLevel = reorderLevel,
            StandardCost = standardCost,
            IsActive = true
        };
    }

    public void UpdateDetails(
        Guid categoryId,
        string sku,
        string name,
        string unit,
        int reorderLevel,
        decimal standardCost,
        string? description = null)
    {
        Guard.AgainstEmpty(categoryId, nameof(categoryId));
        Guard.AgainstNullOrWhiteSpace(sku, nameof(sku), 64);
        Guard.AgainstNullOrWhiteSpace(name, nameof(name), 200);
        Guard.AgainstNullOrWhiteSpace(unit, nameof(unit), 32);
        Guard.AgainstNegative(reorderLevel, nameof(reorderLevel));
        Guard.AgainstNegative(standardCost, nameof(standardCost));

        CategoryId = categoryId;
        Sku = sku.Trim().ToUpperInvariant();
        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Unit = unit.Trim().ToUpperInvariant();
        ReorderLevel = reorderLevel;
        StandardCost = standardCost;
        Touch();
    }

    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }

    public void Activate()
    {
        IsActive = true;
        Touch();
    }
}

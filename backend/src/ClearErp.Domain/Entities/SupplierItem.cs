using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class SupplierItem : BaseEntity
{
    public Guid SupplierId { get; set; }
    public Guid ItemId { get; set; }
    public string SupplierSku { get; set; } = string.Empty;

    public Supplier? Supplier { get; set; }
    public Item? Item { get; set; }

    public static SupplierItem Create(Guid supplierId, Guid itemId, string supplierSku)
    {
        Guard.AgainstEmpty(supplierId, nameof(supplierId));
        Guard.AgainstEmpty(itemId, nameof(itemId));
        Guard.AgainstNullOrWhiteSpace(supplierSku, nameof(supplierSku), 64);

        return new SupplierItem
        {
            SupplierId = supplierId,
            ItemId = itemId,
            SupplierSku = supplierSku.Trim().ToUpperInvariant()
        };
    }
}

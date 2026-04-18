using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClearErp.Domain.Entities;

namespace ClearErp.Infrastructure.Persistence.Configurations;

public sealed class SupplierItemConfiguration : IEntityTypeConfiguration<SupplierItem>
{
    public void Configure(EntityTypeBuilder<SupplierItem> builder)
    {
        builder.ToTable("supplier_items");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SupplierSku).HasMaxLength(64).IsRequired();

        builder.HasIndex(x => new { x.SupplierId, x.ItemId }).IsUnique();

        builder.HasOne(x => x.Supplier)
            .WithMany(x => x.SupplierItems)
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Item)
            .WithMany(x => x.SupplierItems)
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

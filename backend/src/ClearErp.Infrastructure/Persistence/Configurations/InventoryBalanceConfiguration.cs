using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClearErp.Domain.Entities;

namespace ClearErp.Infrastructure.Persistence.Configurations;

public sealed class InventoryBalanceConfiguration : IEntityTypeConfiguration<InventoryBalance>
{
    public void Configure(EntityTypeBuilder<InventoryBalance> builder)
    {
        builder.ToTable("inventory_balances");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasIndex(x => new { x.ItemId, x.WarehouseId, x.LocationId }).IsUnique();

        builder.HasOne(x => x.Item)
            .WithMany(x => x.InventoryBalances)
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Warehouse)
            .WithMany(x => x.InventoryBalances)
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Location)
            .WithMany(x => x.InventoryBalances)
            .HasForeignKey(x => x.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

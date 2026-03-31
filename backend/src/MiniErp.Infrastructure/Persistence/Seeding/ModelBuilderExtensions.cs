using Microsoft.EntityFrameworkCore;
using MiniErp.Domain.Entities;
using MiniErp.Domain.Enums;

namespace MiniErp.Infrastructure.Persistence.Seeding;

public static class ModelBuilderExtensions
{
    public static void ApplySeedData(this ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = SeedConstants.AdminRoleId, Name = RoleName.Admin.ToString(), CreatedAt = seedCreatedAt },
            new Role { Id = SeedConstants.InventoryManagerRoleId, Name = RoleName.InventoryManager.ToString(), CreatedAt = seedCreatedAt },
            new Role { Id = SeedConstants.WarehouseStaffRoleId, Name = RoleName.WarehouseStaff.ToString(), CreatedAt = seedCreatedAt },
            new Role { Id = SeedConstants.ViewerRoleId, Name = RoleName.Viewer.ToString(), CreatedAt = seedCreatedAt });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = SeedConstants.AdminUserId,
                Email = "admin@minierp.local",
                PasswordHash = "PBKDF2$demo-seeded-admin-password-change-me",
                FullName = "System Administrator",
                IsActive = true,
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                UserId = SeedConstants.AdminUserId,
                RoleId = SeedConstants.AdminRoleId
            });

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = SeedConstants.SeatingCategoryId, Name = "Seating", CreatedAt = seedCreatedAt },
            new Category { Id = SeedConstants.StorageCategoryId, Name = "Storage", CreatedAt = seedCreatedAt },
            new Category { Id = SeedConstants.ComponentsCategoryId, Name = "Components", CreatedAt = seedCreatedAt });

        modelBuilder.Entity<Supplier>().HasData(
            new Supplier
            {
                Id = SeedConstants.AcmeSupplierId,
                Name = "Acme Industrial Supply",
                ContactName = "Jordan Lee",
                Email = "orders@acme-industrial.example",
                Phone = "555-0100",
                IsActive = true,
                CreatedAt = seedCreatedAt
            },
            new Supplier
            {
                Id = SeedConstants.NorthwoodSupplierId,
                Name = "Northwood Components",
                ContactName = "Taylor Smith",
                Email = "sales@northwood.example",
                Phone = "555-0110",
                IsActive = true,
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<Warehouse>().HasData(
            new Warehouse
            {
                Id = SeedConstants.MainWarehouseId,
                Name = "Main Warehouse",
                Code = "MAIN",
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<Location>().HasData(
            new Location
            {
                Id = SeedConstants.MainAisleLocationId,
                WarehouseId = SeedConstants.MainWarehouseId,
                Name = "Aisle A1",
                Code = "A1",
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<Item>().HasData(
            new Item
            {
                Id = SeedConstants.TaskChairItemId,
                CategoryId = SeedConstants.SeatingCategoryId,
                Sku = "CHR-1001",
                Name = "Task Chair",
                Description = "Adjustable office task chair",
                Unit = "EA",
                ReorderLevel = 10,
                StandardCost = 129.99m,
                IsActive = true,
                CreatedAt = seedCreatedAt
            },
            new Item
            {
                Id = SeedConstants.FilingCabinetItemId,
                CategoryId = SeedConstants.StorageCategoryId,
                Sku = "CAB-2001",
                Name = "Filing Cabinet",
                Description = "Three-drawer steel filing cabinet",
                Unit = "EA",
                ReorderLevel = 5,
                StandardCost = 249.00m,
                IsActive = true,
                CreatedAt = seedCreatedAt
            },
            new Item
            {
                Id = SeedConstants.DeskLegKitItemId,
                CategoryId = SeedConstants.ComponentsCategoryId,
                Sku = "KIT-3001",
                Name = "Desk Leg Kit",
                Description = "Set of four metal desk legs",
                Unit = "SET",
                ReorderLevel = 20,
                StandardCost = 58.50m,
                IsActive = true,
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<InventoryBalance>().HasData(
            new InventoryBalance
            {
                Id = Guid.Parse("80000000-0000-0000-0000-000000000001"),
                ItemId = SeedConstants.TaskChairItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                QuantityOnHand = 18,
                QuantityReserved = 2,
                QuantityAvailable = 16,
                CreatedAt = seedCreatedAt
            },
            new InventoryBalance
            {
                Id = Guid.Parse("80000000-0000-0000-0000-000000000002"),
                ItemId = SeedConstants.FilingCabinetItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                QuantityOnHand = 7,
                QuantityReserved = 1,
                QuantityAvailable = 6,
                CreatedAt = seedCreatedAt
            },
            new InventoryBalance
            {
                Id = Guid.Parse("80000000-0000-0000-0000-000000000003"),
                ItemId = SeedConstants.DeskLegKitItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                QuantityOnHand = 32,
                QuantityReserved = 4,
                QuantityAvailable = 28,
                CreatedAt = seedCreatedAt
            });
    }
}

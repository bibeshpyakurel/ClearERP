using Microsoft.EntityFrameworkCore;
using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;

namespace ClearErp.Infrastructure.Persistence.Seeding;

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
                Email = "admin@clearerp.local",
                PasswordHash = "100000.D7/na2fAshvnIG4dYnDY3w==.r1Wxdl0Svjd+hM1EFn3gwvocm0IuZvGY0HTcOEAuGUM=",
                FullName = "System Administrator",
                IsActive = true,
                CreatedAt = seedCreatedAt
            },
            new User
            {
                Id = SeedConstants.WarehouseUserId,
                Email = "warehouse@clearerp.local",
                PasswordHash = "100000.yjJgxVnzj85lVCaN8of6NA==.NTw4Z2FymPSWFp5BzyaC8J1F9NpH6x8el2ST+FgofYE=",
                FullName = "Warehouse Operator",
                IsActive = true,
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                UserId = SeedConstants.AdminUserId,
                RoleId = SeedConstants.AdminRoleId
            },
            new UserRole
            {
                UserId = SeedConstants.WarehouseUserId,
                RoleId = SeedConstants.WarehouseStaffRoleId
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
            },
            new Supplier
            {
                Id = SeedConstants.EastlakeSupplierId,
                Name = "Eastlake Office Furnishings",
                ContactName = "Morgan Patel",
                Email = "account-team@eastlake.example",
                Phone = "555-0120",
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
            },
            new Item
            {
                Id = SeedConstants.LoungeChairItemId,
                CategoryId = SeedConstants.SeatingCategoryId,
                Sku = "LNG-4001",
                Name = "Lounge Chair",
                Description = "Soft seating lounge chair for waiting areas",
                Unit = "EA",
                ReorderLevel = 6,
                StandardCost = 219.00m,
                IsActive = true,
                CreatedAt = seedCreatedAt
            },
            new Item
            {
                Id = SeedConstants.MobilePedestalItemId,
                CategoryId = SeedConstants.StorageCategoryId,
                Sku = "PED-5001",
                Name = "Mobile Pedestal",
                Description = "Lockable under-desk storage pedestal",
                Unit = "EA",
                ReorderLevel = 8,
                StandardCost = 179.00m,
                IsActive = true,
                CreatedAt = seedCreatedAt
            },
            new Item
            {
                Id = SeedConstants.ConferenceTableBaseItemId,
                CategoryId = SeedConstants.ComponentsCategoryId,
                Sku = "BAS-6001",
                Name = "Conference Table Base",
                Description = "Powder-coated steel base for conference tables",
                Unit = "EA",
                ReorderLevel = 4,
                StandardCost = 84.00m,
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
                QuantityOnHand = 4,
                QuantityReserved = 1,
                QuantityAvailable = 3,
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
            },
            new InventoryBalance
            {
                Id = Guid.Parse("80000000-0000-0000-0000-000000000004"),
                ItemId = SeedConstants.LoungeChairItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                QuantityOnHand = 5,
                QuantityReserved = 3,
                QuantityAvailable = 2,
                CreatedAt = seedCreatedAt
            },
            new InventoryBalance
            {
                Id = Guid.Parse("80000000-0000-0000-0000-000000000005"),
                ItemId = SeedConstants.MobilePedestalItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                QuantityOnHand = 14,
                QuantityReserved = 2,
                QuantityAvailable = 12,
                CreatedAt = seedCreatedAt
            },
            new InventoryBalance
            {
                Id = Guid.Parse("80000000-0000-0000-0000-000000000006"),
                ItemId = SeedConstants.ConferenceTableBaseItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                QuantityOnHand = 11,
                QuantityReserved = 1,
                QuantityAvailable = 10,
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<SupplierItem>().HasData(
            new SupplierItem
            {
                Id = SeedConstants.AcmeTaskChairSupplierItemId,
                SupplierId = SeedConstants.AcmeSupplierId,
                ItemId = SeedConstants.TaskChairItemId,
                SupplierSku = "ACME-CHR-1001",
                CreatedAt = seedCreatedAt
            },
            new SupplierItem
            {
                Id = SeedConstants.AcmeCabinetSupplierItemId,
                SupplierId = SeedConstants.AcmeSupplierId,
                ItemId = SeedConstants.FilingCabinetItemId,
                SupplierSku = "ACME-CAB-2001",
                CreatedAt = seedCreatedAt
            },
            new SupplierItem
            {
                Id = SeedConstants.NorthwoodDeskLegSupplierItemId,
                SupplierId = SeedConstants.NorthwoodSupplierId,
                ItemId = SeedConstants.DeskLegKitItemId,
                SupplierSku = "NW-KIT-3001",
                CreatedAt = seedCreatedAt
            },
            new SupplierItem
            {
                Id = SeedConstants.EastlakeLoungeChairSupplierItemId,
                SupplierId = SeedConstants.EastlakeSupplierId,
                ItemId = SeedConstants.LoungeChairItemId,
                SupplierSku = "EL-LNG-4001",
                CreatedAt = seedCreatedAt
            },
            new SupplierItem
            {
                Id = SeedConstants.EastlakePedestalSupplierItemId,
                SupplierId = SeedConstants.EastlakeSupplierId,
                ItemId = SeedConstants.MobilePedestalItemId,
                SupplierSku = "EL-PED-5001",
                CreatedAt = seedCreatedAt
            },
            new SupplierItem
            {
                Id = SeedConstants.NorthwoodConferenceBaseSupplierItemId,
                SupplierId = SeedConstants.NorthwoodSupplierId,
                ItemId = SeedConstants.ConferenceTableBaseItemId,
                SupplierSku = "NW-BAS-6001",
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<PurchaseOrder>().HasData(
            new PurchaseOrder
            {
                Id = SeedConstants.DraftPurchaseOrderId,
                PoNumber = "PO-2026-001",
                SupplierId = SeedConstants.AcmeSupplierId,
                Status = PurchaseOrderStatus.Draft,
                OrderDate = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                ExpectedDate = new DateTime(2026, 4, 5, 0, 0, 0, DateTimeKind.Utc),
                CreatedByUserId = SeedConstants.AdminUserId,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrder
            {
                Id = SeedConstants.ApprovedPurchaseOrderId,
                PoNumber = "PO-2026-002",
                SupplierId = SeedConstants.NorthwoodSupplierId,
                Status = PurchaseOrderStatus.Approved,
                OrderDate = new DateTime(2026, 3, 22, 0, 0, 0, DateTimeKind.Utc),
                ExpectedDate = new DateTime(2026, 4, 8, 0, 0, 0, DateTimeKind.Utc),
                CreatedByUserId = SeedConstants.AdminUserId,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrder
            {
                Id = SeedConstants.PartialPurchaseOrderId,
                PoNumber = "PO-2026-003",
                SupplierId = SeedConstants.AcmeSupplierId,
                Status = PurchaseOrderStatus.PartiallyReceived,
                OrderDate = new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc),
                ExpectedDate = new DateTime(2026, 3, 28, 0, 0, 0, DateTimeKind.Utc),
                CreatedByUserId = SeedConstants.AdminUserId,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrder
            {
                Id = SeedConstants.CompletedPurchaseOrderId,
                PoNumber = "PO-2026-004",
                SupplierId = SeedConstants.NorthwoodSupplierId,
                Status = PurchaseOrderStatus.Completed,
                OrderDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                ExpectedDate = new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc),
                CreatedByUserId = SeedConstants.AdminUserId,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrder
            {
                Id = SeedConstants.CancelledPurchaseOrderId,
                PoNumber = "PO-2026-005",
                SupplierId = SeedConstants.AcmeSupplierId,
                Status = PurchaseOrderStatus.Cancelled,
                OrderDate = new DateTime(2026, 3, 12, 0, 0, 0, DateTimeKind.Utc),
                ExpectedDate = new DateTime(2026, 3, 21, 0, 0, 0, DateTimeKind.Utc),
                CreatedByUserId = SeedConstants.AdminUserId,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrder
            {
                Id = SeedConstants.EastlakeApprovedPurchaseOrderId,
                PoNumber = "PO-2026-006",
                SupplierId = SeedConstants.EastlakeSupplierId,
                Status = PurchaseOrderStatus.Approved,
                OrderDate = new DateTime(2026, 3, 27, 0, 0, 0, DateTimeKind.Utc),
                ExpectedDate = new DateTime(2026, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                CreatedByUserId = SeedConstants.AdminUserId,
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<PurchaseOrderLine>().HasData(
            new PurchaseOrderLine
            {
                Id = SeedConstants.DraftPurchaseOrderLineId,
                PurchaseOrderId = SeedConstants.DraftPurchaseOrderId,
                ItemId = SeedConstants.FilingCabinetItemId,
                OrderedQuantity = 6,
                ReceivedQuantity = 0,
                UnitCost = 235.00m,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrderLine
            {
                Id = SeedConstants.DraftPurchaseOrderSecondLineId,
                PurchaseOrderId = SeedConstants.DraftPurchaseOrderId,
                ItemId = SeedConstants.LoungeChairItemId,
                OrderedQuantity = 4,
                ReceivedQuantity = 0,
                UnitCost = 210.00m,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrderLine
            {
                Id = SeedConstants.ApprovedPurchaseOrderLineId,
                PurchaseOrderId = SeedConstants.ApprovedPurchaseOrderId,
                ItemId = SeedConstants.DeskLegKitItemId,
                OrderedQuantity = 12,
                ReceivedQuantity = 0,
                UnitCost = 54.75m,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrderLine
            {
                Id = SeedConstants.ApprovedPurchaseOrderSecondLineId,
                PurchaseOrderId = SeedConstants.ApprovedPurchaseOrderId,
                ItemId = SeedConstants.ConferenceTableBaseItemId,
                OrderedQuantity = 6,
                ReceivedQuantity = 0,
                UnitCost = 84.00m,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrderLine
            {
                Id = SeedConstants.PartialPurchaseOrderLineId,
                PurchaseOrderId = SeedConstants.PartialPurchaseOrderId,
                ItemId = SeedConstants.TaskChairItemId,
                OrderedQuantity = 10,
                ReceivedQuantity = 4,
                UnitCost = 121.50m,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrderLine
            {
                Id = SeedConstants.CompletedPurchaseOrderLineId,
                PurchaseOrderId = SeedConstants.CompletedPurchaseOrderId,
                ItemId = SeedConstants.DeskLegKitItemId,
                OrderedQuantity = 8,
                ReceivedQuantity = 8,
                UnitCost = 52.00m,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrderLine
            {
                Id = SeedConstants.CancelledPurchaseOrderLineId,
                PurchaseOrderId = SeedConstants.CancelledPurchaseOrderId,
                ItemId = SeedConstants.TaskChairItemId,
                OrderedQuantity = 3,
                ReceivedQuantity = 0,
                UnitCost = 125.00m,
                CreatedAt = seedCreatedAt
            },
            new PurchaseOrderLine
            {
                Id = SeedConstants.EastlakeApprovedPurchaseOrderLineId,
                PurchaseOrderId = SeedConstants.EastlakeApprovedPurchaseOrderId,
                ItemId = SeedConstants.MobilePedestalItemId,
                OrderedQuantity = 10,
                ReceivedQuantity = 0,
                UnitCost = 176.50m,
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<GoodsReceipt>().HasData(
            new GoodsReceipt
            {
                Id = SeedConstants.PartialGoodsReceiptId,
                PurchaseOrderId = SeedConstants.PartialPurchaseOrderId,
                ReceiptNumber = "GR-2026-003",
                ReceivedAt = new DateTime(2026, 3, 25, 14, 0, 0, DateTimeKind.Utc),
                ReceivedByUserId = SeedConstants.WarehouseUserId,
                CreatedAt = seedCreatedAt
            },
            new GoodsReceipt
            {
                Id = SeedConstants.CompletedGoodsReceiptId,
                PurchaseOrderId = SeedConstants.CompletedPurchaseOrderId,
                ReceiptNumber = "GR-2026-004",
                ReceivedAt = new DateTime(2026, 3, 16, 10, 30, 0, DateTimeKind.Utc),
                ReceivedByUserId = SeedConstants.WarehouseUserId,
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<GoodsReceiptLine>().HasData(
            new GoodsReceiptLine
            {
                Id = SeedConstants.PartialGoodsReceiptLineId,
                GoodsReceiptId = SeedConstants.PartialGoodsReceiptId,
                PurchaseOrderLineId = SeedConstants.PartialPurchaseOrderLineId,
                ItemId = SeedConstants.TaskChairItemId,
                ReceivedQuantity = 4,
                CreatedAt = seedCreatedAt
            },
            new GoodsReceiptLine
            {
                Id = SeedConstants.CompletedGoodsReceiptLineId,
                GoodsReceiptId = SeedConstants.CompletedGoodsReceiptId,
                PurchaseOrderLineId = SeedConstants.CompletedPurchaseOrderLineId,
                ItemId = SeedConstants.DeskLegKitItemId,
                ReceivedQuantity = 8,
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<InventoryTransaction>().HasData(
            new InventoryTransaction
            {
                Id = SeedConstants.ReceiptTransactionId,
                ItemId = SeedConstants.TaskChairItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                TransactionType = InventoryTransactionType.Receipt,
                ReferenceType = "GoodsReceipt",
                ReferenceId = SeedConstants.PartialGoodsReceiptId,
                Reason = null,
                QuantityChange = 4,
                BalanceAfter = 18,
                PerformedByUserId = SeedConstants.WarehouseUserId,
                PerformedAt = new DateTime(2026, 3, 25, 14, 0, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new InventoryTransaction
            {
                Id = SeedConstants.IssueTransactionId,
                ItemId = SeedConstants.FilingCabinetItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                TransactionType = InventoryTransactionType.Issue,
                ReferenceType = "ManualIssue",
                ReferenceId = null,
                Reason = "Reserved for municipal office fit-out",
                QuantityChange = -2,
                BalanceAfter = 4,
                PerformedByUserId = SeedConstants.WarehouseUserId,
                PerformedAt = new DateTime(2026, 3, 29, 9, 15, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new InventoryTransaction
            {
                Id = SeedConstants.AdjustmentIncreaseTransactionId,
                ItemId = SeedConstants.DeskLegKitItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                TransactionType = InventoryTransactionType.AdjustmentIncrease,
                ReferenceType = "StockAdjustment",
                ReferenceId = SeedConstants.DeskLegCycleCountAdjustmentId,
                Reason = "Cycle count found extra kit components",
                QuantityChange = 3,
                BalanceAfter = 32,
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 3, 30, 8, 45, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new InventoryTransaction
            {
                Id = SeedConstants.AdjustmentDecreaseTransactionId,
                ItemId = SeedConstants.FilingCabinetItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                TransactionType = InventoryTransactionType.AdjustmentDecrease,
                ReferenceType = "StockAdjustment",
                ReferenceId = SeedConstants.CabinetDamageAdjustmentId,
                Reason = "One cabinet damaged in transit",
                QuantityChange = -1,
                BalanceAfter = 3,
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 3, 30, 11, 0, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new InventoryTransaction
            {
                Id = SeedConstants.PedestalReceiptTransactionId,
                ItemId = SeedConstants.MobilePedestalItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                TransactionType = InventoryTransactionType.Receipt,
                ReferenceType = "VendorDelivery",
                ReferenceId = SeedConstants.EastlakeApprovedPurchaseOrderId,
                Reason = "Rush replenishment for mobile pedestals",
                QuantityChange = 6,
                BalanceAfter = 14,
                PerformedByUserId = SeedConstants.WarehouseUserId,
                PerformedAt = new DateTime(2026, 3, 31, 9, 30, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new InventoryTransaction
            {
                Id = SeedConstants.TaskChairIssueTransactionId,
                ItemId = SeedConstants.TaskChairItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                TransactionType = InventoryTransactionType.Issue,
                ReferenceType = "ShowroomRefresh",
                ReferenceId = null,
                Reason = "Moved chairs to showroom staging area",
                QuantityChange = -2,
                BalanceAfter = 16,
                PerformedByUserId = SeedConstants.WarehouseUserId,
                PerformedAt = new DateTime(2026, 3, 31, 13, 20, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new InventoryTransaction
            {
                Id = SeedConstants.ConferenceBaseAdjustmentTransactionId,
                ItemId = SeedConstants.ConferenceTableBaseItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                TransactionType = InventoryTransactionType.AdjustmentIncrease,
                ReferenceType = "StockAdjustment",
                ReferenceId = SeedConstants.ConferenceBaseAdjustmentId,
                Reason = "Found two extra conference table bases during cycle count",
                QuantityChange = 2,
                BalanceAfter = 11,
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 3, 31, 15, 5, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<StockAdjustment>().HasData(
            new StockAdjustment
            {
                Id = SeedConstants.DeskLegCycleCountAdjustmentId,
                ItemId = SeedConstants.DeskLegKitItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                AdjustmentType = AdjustmentType.Increase,
                QuantityDelta = 3,
                Reason = "Cycle count found extra kit components",
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 3, 30, 8, 45, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new StockAdjustment
            {
                Id = SeedConstants.CabinetDamageAdjustmentId,
                ItemId = SeedConstants.FilingCabinetItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                AdjustmentType = AdjustmentType.Decrease,
                QuantityDelta = 1,
                Reason = "One cabinet damaged in transit",
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 3, 30, 11, 0, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new StockAdjustment
            {
                Id = SeedConstants.ConferenceBaseAdjustmentId,
                ItemId = SeedConstants.ConferenceTableBaseItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                AdjustmentType = AdjustmentType.Increase,
                QuantityDelta = 2,
                Reason = "Found two extra conference table bases during cycle count",
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 3, 31, 15, 5, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            });

        modelBuilder.Entity<AuditLog>().HasData(
            new AuditLog
            {
                Id = SeedConstants.PurchaseOrderCreatedAuditLogId,
                Action = "PurchaseOrderCreated",
                EntityName = nameof(PurchaseOrder),
                EntityId = SeedConstants.ApprovedPurchaseOrderId,
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 3, 22, 9, 30, 0, DateTimeKind.Utc),
                Details = "Created approved demo purchase order PO-2026-002.",
                CreatedAt = seedCreatedAt
            },
            new AuditLog
            {
                Id = SeedConstants.GoodsReceiptAuditLogId,
                Action = "GoodsReceiptPosted",
                EntityName = nameof(GoodsReceipt),
                EntityId = SeedConstants.PartialGoodsReceiptId,
                PerformedByUserId = SeedConstants.WarehouseUserId,
                PerformedAt = new DateTime(2026, 3, 25, 14, 0, 0, DateTimeKind.Utc),
                Details = "Received 4 task chairs against PO-2026-003.",
                CreatedAt = seedCreatedAt
            },
            new AuditLog
            {
                Id = SeedConstants.StockIssueAuditLogId,
                Action = "StockIssued",
                EntityName = nameof(InventoryTransaction),
                EntityId = SeedConstants.IssueTransactionId,
                PerformedByUserId = SeedConstants.WarehouseUserId,
                PerformedAt = new DateTime(2026, 3, 29, 9, 15, 0, DateTimeKind.Utc),
                Details = "Issued two filing cabinets for municipal office fit-out.",
                CreatedAt = seedCreatedAt
            },
            new AuditLog
            {
                Id = SeedConstants.StockAdjustmentAuditLogId,
                Action = "StockAdjusted",
                EntityName = nameof(StockAdjustment),
                EntityId = SeedConstants.CabinetDamageAdjustmentId,
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 3, 30, 11, 0, 0, DateTimeKind.Utc),
                Details = "Recorded a damaged filing cabinet after inbound inspection.",
                CreatedAt = seedCreatedAt
            },
            new AuditLog
            {
                Id = SeedConstants.EastlakePurchaseOrderAuditLogId,
                Action = "PurchaseOrderCreated",
                EntityName = nameof(PurchaseOrder),
                EntityId = SeedConstants.EastlakeApprovedPurchaseOrderId,
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 3, 27, 10, 0, 0, DateTimeKind.Utc),
                Details = "Created approved demo purchase order PO-2026-006 for Eastlake Office Furnishings.",
                CreatedAt = seedCreatedAt
            },
            new AuditLog
            {
                Id = SeedConstants.PedestalReceiptAuditLogId,
                Action = "GoodsReceiptPosted",
                EntityName = nameof(InventoryTransaction),
                EntityId = SeedConstants.PedestalReceiptTransactionId,
                PerformedByUserId = SeedConstants.WarehouseUserId,
                PerformedAt = new DateTime(2026, 3, 31, 9, 30, 0, DateTimeKind.Utc),
                Details = "Recorded rush replenishment receipt for mobile pedestals.",
                CreatedAt = seedCreatedAt
            },
            new AuditLog
            {
                Id = SeedConstants.TaskChairIssueAuditLogId,
                Action = "StockIssued",
                EntityName = nameof(InventoryTransaction),
                EntityId = SeedConstants.TaskChairIssueTransactionId,
                PerformedByUserId = SeedConstants.WarehouseUserId,
                PerformedAt = new DateTime(2026, 3, 31, 13, 20, 0, DateTimeKind.Utc),
                Details = "Issued two task chairs for showroom refresh.",
                CreatedAt = seedCreatedAt
            },
            new AuditLog
            {
                Id = SeedConstants.ConferenceBaseAdjustmentAuditLogId,
                Action = "StockAdjusted",
                EntityName = nameof(StockAdjustment),
                EntityId = SeedConstants.ConferenceBaseAdjustmentId,
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 3, 31, 15, 5, 0, DateTimeKind.Utc),
                Details = "Cycle count increased conference table base inventory by two units.",
                CreatedAt = seedCreatedAt
            });
    }
}

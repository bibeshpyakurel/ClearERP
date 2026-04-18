using Microsoft.EntityFrameworkCore;
using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;
using ClearErp.Infrastructure.Persistence;
using ClearErp.Infrastructure.Persistence.Seeding;
using ClearErp.Infrastructure.Services;

namespace ClearErp.UnitTests.Services;

public sealed class ReportingServiceTests
{
    [Fact]
    public async Task GetLowStockReportAsync_Should_ReturnOnlyLowStockItems()
    {
        await using var dbContext = CreateDbContext();
        SeedReportingData(dbContext);

        var service = new ReportingService(dbContext);

        var report = await service.GetLowStockReportAsync();

        Assert.Single(report);
        Assert.Equal(SeedConstants.TaskChairItemId, report[0].ItemId);
        Assert.Equal(5, report[0].Shortfall);
    }

    [Fact]
    public async Task GetStockSummaryAsync_Should_ReturnAggregatedQuantities()
    {
        await using var dbContext = CreateDbContext();
        SeedReportingData(dbContext);

        var service = new ReportingService(dbContext);

        var report = await service.GetStockSummaryAsync();

        Assert.Equal(2, report.TotalTrackedItems);
        Assert.Equal(18, report.TotalQuantityOnHand);
        Assert.Equal(3, report.TotalQuantityReserved);
        Assert.Equal(15, report.TotalQuantityAvailable);
        Assert.Equal(1, report.LowStockItemCount);
    }

    [Fact]
    public async Task GetStockValuationAsync_Should_ReturnTotalInventoryValue()
    {
        await using var dbContext = CreateDbContext();
        SeedReportingData(dbContext);

        var service = new ReportingService(dbContext);

        var report = await service.GetStockValuationAsync();

        Assert.Equal(2450m, report.TotalInventoryValue);
        Assert.Equal(2, report.Items.Count);
    }

    [Fact]
    public async Task GetPurchaseOrderSummaryAsync_Should_ReturnCountsAndOpenValue()
    {
        await using var dbContext = CreateDbContext();
        SeedReportingData(dbContext);

        var service = new ReportingService(dbContext);

        var report = await service.GetPurchaseOrderSummaryAsync();

        Assert.Equal(1, report.DraftCount);
        Assert.Equal(1, report.ApprovedCount);
        Assert.Equal(0, report.PartiallyReceivedCount);
        Assert.Equal(1, report.CompletedCount);
        Assert.Equal(1, report.CancelledCount);
        Assert.Equal(700m, report.TotalOpenPurchaseOrderValue);
    }

    [Fact]
    public async Task GetRecentTransactionsAsync_Should_ReturnMostRecentTransactionsFirst()
    {
        await using var dbContext = CreateDbContext();
        SeedReportingData(dbContext);

        var service = new ReportingService(dbContext);

        var report = await service.GetRecentTransactionsAsync(2);

        Assert.Equal(2, report.Count);
        Assert.True(report[0].PerformedAt >= report[1].PerformedAt);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static void SeedReportingData(ApplicationDbContext dbContext)
    {
        dbContext.Categories.Add(new Category
        {
            Id = SeedConstants.SeatingCategoryId,
            Name = "Seating"
        });

        dbContext.Items.AddRange(
            new Item
            {
                Id = SeedConstants.TaskChairItemId,
                CategoryId = SeedConstants.SeatingCategoryId,
                Sku = "CHR-1001",
                Name = "Task Chair",
                Unit = "EA",
                ReorderLevel = 10,
                StandardCost = 100m,
                IsActive = true
            },
            new Item
            {
                Id = SeedConstants.FilingCabinetItemId,
                CategoryId = SeedConstants.SeatingCategoryId,
                Sku = "CAB-2001",
                Name = "Filing Cabinet",
                Unit = "EA",
                ReorderLevel = 3,
                StandardCost = 150m,
                IsActive = true
            });

        dbContext.Warehouses.Add(new Warehouse
        {
            Id = SeedConstants.MainWarehouseId,
            Name = "Main Warehouse",
            Code = "MAIN"
        });

        dbContext.Locations.Add(new Location
        {
            Id = SeedConstants.MainAisleLocationId,
            WarehouseId = SeedConstants.MainWarehouseId,
            Name = "Aisle A1",
            Code = "A1"
        });

        dbContext.InventoryBalances.AddRange(
            new InventoryBalance
            {
                Id = Guid.NewGuid(),
                ItemId = SeedConstants.TaskChairItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                QuantityOnHand = 5,
                QuantityReserved = 0,
                QuantityAvailable = 5
            },
            new InventoryBalance
            {
                Id = Guid.NewGuid(),
                ItemId = SeedConstants.FilingCabinetItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                QuantityOnHand = 13,
                QuantityReserved = 3,
                QuantityAvailable = 10
            });

        dbContext.PurchaseOrders.AddRange(
            CreatePurchaseOrder(Guid.NewGuid(), "PO-DRAFT", PurchaseOrderStatus.Draft, 2, 100m),
            CreatePurchaseOrder(Guid.NewGuid(), "PO-APPROVED", PurchaseOrderStatus.Approved, 5, 100m),
            CreatePurchaseOrder(Guid.NewGuid(), "PO-COMPLETE", PurchaseOrderStatus.Completed, 1, 100m),
            CreatePurchaseOrder(Guid.NewGuid(), "PO-CANCEL", PurchaseOrderStatus.Cancelled, 1, 100m));

        dbContext.InventoryTransactions.AddRange(
            new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ItemId = SeedConstants.TaskChairItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                TransactionType = InventoryTransactionType.Issue,
                ReferenceType = "ManualIssue",
                Reason = "Consumed",
                QuantityChange = -2,
                BalanceAfter = 8,
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 4, 2, 10, 0, 0, DateTimeKind.Utc)
            },
            new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ItemId = SeedConstants.FilingCabinetItemId,
                WarehouseId = SeedConstants.MainWarehouseId,
                LocationId = SeedConstants.MainAisleLocationId,
                TransactionType = InventoryTransactionType.Receipt,
                ReferenceType = "GoodsReceipt",
                Reason = null,
                QuantityChange = 5,
                BalanceAfter = 13,
                PerformedByUserId = SeedConstants.AdminUserId,
                PerformedAt = new DateTime(2026, 4, 3, 10, 0, 0, DateTimeKind.Utc)
            });

        dbContext.SaveChanges();
    }

    private static PurchaseOrder CreatePurchaseOrder(Guid id, string poNumber, PurchaseOrderStatus status, int quantity, decimal unitCost)
    {
        var purchaseOrder = new PurchaseOrder
        {
            Id = id,
            PoNumber = poNumber,
            SupplierId = SeedConstants.AcmeSupplierId,
            Status = status,
            OrderDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
            CreatedByUserId = SeedConstants.AdminUserId
        };

        purchaseOrder.Lines.Add(new PurchaseOrderLine
        {
            Id = Guid.NewGuid(),
            PurchaseOrderId = id,
            ItemId = SeedConstants.TaskChairItemId,
            OrderedQuantity = quantity,
            ReceivedQuantity = status == PurchaseOrderStatus.Completed ? quantity : 0,
            UnitCost = unitCost
        });

        return purchaseOrder;
    }
}

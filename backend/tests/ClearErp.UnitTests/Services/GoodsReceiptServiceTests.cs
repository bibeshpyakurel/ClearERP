using Microsoft.EntityFrameworkCore;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;
using ClearErp.Infrastructure.Persistence;
using ClearErp.Infrastructure.Persistence.Repositories;
using ClearErp.Infrastructure.Persistence.Seeding;
using ClearErp.Infrastructure.Services;

namespace ClearErp.UnitTests.Services;

public sealed class GoodsReceiptServiceTests
{
    [Fact]
    public async Task ReceiveAgainstPurchaseOrderAsync_Should_UpdateBalances_CreateReceipt_And_AdvanceStatus()
    {
        await using var dbContext = CreateDbContext();
        SeedReceiptData(dbContext, PurchaseOrderStatus.Approved);

        var service = CreateService(dbContext);
        var poLineId = await dbContext.PurchaseOrderLines.Select(x => x.Id).SingleAsync();

        var receipt = await service.ReceiveAgainstPurchaseOrderAsync(
            SeedConstants.ApprovedPurchaseOrderId,
            "GR-1001",
            SeedConstants.WarehouseUserId,
            new DateTime(2026, 4, 1, 15, 0, 0, DateTimeKind.Utc),
            new[]
            {
                new Application.Common.Interfaces.Services.PostGoodsReceiptLineRequest(
                    poLineId,
                    SeedConstants.TaskChairItemId,
                    5,
                    SeedConstants.MainWarehouseId,
                    SeedConstants.MainAisleLocationId)
            });

        var purchaseOrder = await dbContext.PurchaseOrders.Include(x => x.Lines).SingleAsync(x => x.Id == SeedConstants.ApprovedPurchaseOrderId);
        var balance = await dbContext.InventoryBalances.SingleAsync(x => x.ItemId == SeedConstants.TaskChairItemId);
        var transaction = await dbContext.InventoryTransactions.SingleAsync();
        var goodsReceipt = await dbContext.GoodsReceipts.Include(x => x.Lines).SingleAsync();
        var auditLog = await dbContext.AuditLogs.SingleAsync();

        Assert.Equal("GR-1001", receipt.ReceiptNumber);
        Assert.Equal(PurchaseOrderStatus.Completed, purchaseOrder.Status);
        Assert.Equal(5, purchaseOrder.Lines.Single().ReceivedQuantity);
        Assert.Equal(15, balance.QuantityOnHand);
        Assert.Equal(InventoryTransactionType.Receipt, transaction.TransactionType);
        Assert.Single(goodsReceipt.Lines);
        Assert.Equal("GoodsReceiptPosted", auditLog.Action);
    }

    [Fact]
    public async Task ReceiveAgainstPurchaseOrderAsync_Should_SetStatusToPartiallyReceived_When_LineIsNotFullyReceived()
    {
        await using var dbContext = CreateDbContext();
        SeedReceiptData(dbContext, PurchaseOrderStatus.Approved);

        var service = CreateService(dbContext);
        var poLineId = await dbContext.PurchaseOrderLines.Select(x => x.Id).SingleAsync();

        await service.ReceiveAgainstPurchaseOrderAsync(
            SeedConstants.ApprovedPurchaseOrderId,
            "GR-1001-PARTIAL",
            SeedConstants.WarehouseUserId,
            DateTime.UtcNow,
            new[]
            {
                new Application.Common.Interfaces.Services.PostGoodsReceiptLineRequest(
                    poLineId,
                    SeedConstants.TaskChairItemId,
                    3,
                    SeedConstants.MainWarehouseId,
                    SeedConstants.MainAisleLocationId)
            });

        var purchaseOrder = await dbContext.PurchaseOrders.Include(x => x.Lines).SingleAsync(x => x.Id == SeedConstants.ApprovedPurchaseOrderId);

        Assert.Equal(PurchaseOrderStatus.PartiallyReceived, purchaseOrder.Status);
        Assert.Equal(3, purchaseOrder.Lines.Single().ReceivedQuantity);
    }

    [Fact]
    public async Task ReceiveAgainstPurchaseOrderAsync_Should_Throw_When_ItemDoesNotMatchLine()
    {
        await using var dbContext = CreateDbContext();
        SeedReceiptData(dbContext, PurchaseOrderStatus.Approved);

        var service = CreateService(dbContext);
        var poLineId = await dbContext.PurchaseOrderLines.Select(x => x.Id).SingleAsync();

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.ReceiveAgainstPurchaseOrderAsync(
                SeedConstants.ApprovedPurchaseOrderId,
                "GR-1002",
                SeedConstants.WarehouseUserId,
                DateTime.UtcNow,
                new[]
                {
                    new Application.Common.Interfaces.Services.PostGoodsReceiptLineRequest(
                        poLineId,
                        SeedConstants.FilingCabinetItemId,
                        1,
                        SeedConstants.MainWarehouseId,
                        SeedConstants.MainAisleLocationId)
                }));

        Assert.Equal("Receipt item does not match the purchase order line item.", exception.Message);
    }

    [Fact]
    public async Task ReceiveAgainstPurchaseOrderAsync_Should_Throw_When_QuantityExceedsOrderedAmount()
    {
        await using var dbContext = CreateDbContext();
        SeedReceiptData(dbContext, PurchaseOrderStatus.Approved);

        var service = CreateService(dbContext);
        var poLineId = await dbContext.PurchaseOrderLines.Select(x => x.Id).SingleAsync();

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.ReceiveAgainstPurchaseOrderAsync(
                SeedConstants.ApprovedPurchaseOrderId,
                "GR-1003",
                SeedConstants.WarehouseUserId,
                DateTime.UtcNow,
                new[]
                {
                    new Application.Common.Interfaces.Services.PostGoodsReceiptLineRequest(
                        poLineId,
                        SeedConstants.TaskChairItemId,
                        6,
                        SeedConstants.MainWarehouseId,
                        SeedConstants.MainAisleLocationId)
                }));

        Assert.Equal("Received quantity cannot exceed ordered quantity.", exception.Message);
    }

    [Fact]
    public async Task ReceiveAgainstPurchaseOrderAsync_Should_Throw_When_PurchaseOrderIsClosed()
    {
        await using var dbContext = CreateDbContext();
        SeedReceiptData(dbContext, PurchaseOrderStatus.Completed);

        var service = CreateService(dbContext);
        var poLineId = await dbContext.PurchaseOrderLines.Select(x => x.Id).SingleAsync();

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.ReceiveAgainstPurchaseOrderAsync(
                SeedConstants.ApprovedPurchaseOrderId,
                "GR-1004",
                SeedConstants.WarehouseUserId,
                DateTime.UtcNow,
                new[]
                {
                    new Application.Common.Interfaces.Services.PostGoodsReceiptLineRequest(
                        poLineId,
                        SeedConstants.TaskChairItemId,
                        1,
                        SeedConstants.MainWarehouseId,
                        SeedConstants.MainAisleLocationId)
                }));

        Assert.Equal("This purchase order is not receivable.", exception.Message);
    }

    private static GoodsReceiptService CreateService(ApplicationDbContext dbContext) =>
        new(
            new PurchaseOrderRepository(dbContext),
            new InventoryBalanceRepository(dbContext),
            new InventoryTransactionRepository(dbContext),
            new AuditLogRepository(dbContext),
            dbContext);

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static void SeedReceiptData(ApplicationDbContext dbContext, PurchaseOrderStatus status)
    {
        dbContext.Users.AddRange(
            new User
            {
                Id = SeedConstants.AdminUserId,
                Email = "admin@clearerp.local",
                PasswordHash = "hash",
                FullName = "Admin"
            },
            new User
            {
                Id = SeedConstants.WarehouseUserId,
                Email = "warehouse@clearerp.local",
                PasswordHash = "hash",
                FullName = "Warehouse"
            });

        dbContext.Suppliers.Add(new Supplier
        {
            Id = SeedConstants.AcmeSupplierId,
            Name = "Acme",
            ContactName = "Jordan",
            Email = "orders@acme.example",
            Phone = "555-0100",
            IsActive = true
        });

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
                ReorderLevel = 2,
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
                ReorderLevel = 1,
                StandardCost = 200m,
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

        dbContext.InventoryBalances.Add(new InventoryBalance
        {
            Id = Guid.NewGuid(),
            ItemId = SeedConstants.TaskChairItemId,
            WarehouseId = SeedConstants.MainWarehouseId,
            LocationId = SeedConstants.MainAisleLocationId,
            QuantityOnHand = 10,
            QuantityReserved = 0,
            QuantityAvailable = 10
        });

        var purchaseOrder = new PurchaseOrder
        {
            Id = SeedConstants.ApprovedPurchaseOrderId,
            PoNumber = "PO-1001",
            SupplierId = SeedConstants.AcmeSupplierId,
            Status = status,
            OrderDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
            CreatedByUserId = SeedConstants.AdminUserId
        };

        var poLine = new PurchaseOrderLine
        {
            Id = SeedConstants.ApprovedPurchaseOrderLineId,
            PurchaseOrderId = SeedConstants.ApprovedPurchaseOrderId,
            ItemId = SeedConstants.TaskChairItemId,
            OrderedQuantity = 5,
            ReceivedQuantity = 0,
            UnitCost = 100m
        };

        purchaseOrder.Lines.Add(poLine);
        dbContext.PurchaseOrders.Add(purchaseOrder);
        dbContext.PurchaseOrderLines.Add(poLine);
        dbContext.SaveChanges();
    }
}

using Microsoft.EntityFrameworkCore;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;
using ClearErp.Infrastructure.Persistence;
using ClearErp.Infrastructure.Persistence.Repositories;
using ClearErp.Infrastructure.Persistence.Seeding;
using ClearErp.Infrastructure.Services;

namespace ClearErp.UnitTests.Services;

public sealed class InventoryServiceTests
{
    [Fact]
    public async Task IssueStockAsync_Should_DecreaseBalance_And_CreateTransaction()
    {
        await using var dbContext = CreateDbContext();
        SeedBaseData(dbContext);

        var service = CreateService(dbContext);
        var initialQuantityOnHand = (await dbContext.InventoryBalances
            .AsNoTracking()
            .SingleAsync(x => x.ItemId == SeedConstants.TaskChairItemId)).QuantityOnHand;

        await service.IssueStockAsync(
            SeedConstants.TaskChairItemId,
            SeedConstants.MainWarehouseId,
            SeedConstants.MainAisleLocationId,
            3,
            SeedConstants.AdminUserId,
            "ManualIssue",
            "Consumed for showroom staging");

        var updatedBalance = await dbContext.InventoryBalances.SingleAsync(x => x.ItemId == SeedConstants.TaskChairItemId);
        var transaction = await dbContext.InventoryTransactions.SingleAsync();
        var auditLog = await dbContext.AuditLogs.SingleAsync();

        Assert.Equal(initialQuantityOnHand - 3, updatedBalance.QuantityOnHand);
        Assert.Equal(InventoryTransactionType.Issue, transaction.TransactionType);
        Assert.Equal(-3, transaction.QuantityChange);
        Assert.Equal("Consumed for showroom staging", transaction.Reason);
        Assert.Equal(updatedBalance.QuantityOnHand, transaction.BalanceAfter);
        Assert.Equal("StockIssued", auditLog.Action);
    }

    [Fact]
    public async Task IssueStockAsync_Should_Throw_When_QuantityExceedsAvailable()
    {
        await using var dbContext = CreateDbContext();
        SeedBaseData(dbContext);

        var service = CreateService(dbContext);

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.IssueStockAsync(
                SeedConstants.TaskChairItemId,
                SeedConstants.MainWarehouseId,
                SeedConstants.MainAisleLocationId,
                99,
                SeedConstants.AdminUserId,
                "ManualIssue",
                "Attempted over-issue"));

        Assert.Equal("Cannot issue more inventory than is available.", exception.Message);
    }

    [Fact]
    public async Task AdjustStockAsync_Should_CreateStockAdjustment_And_Transaction()
    {
        await using var dbContext = CreateDbContext();
        SeedBaseData(dbContext);

        var service = CreateService(dbContext);

        await service.AdjustStockAsync(
            SeedConstants.TaskChairItemId,
            SeedConstants.MainWarehouseId,
            SeedConstants.MainAisleLocationId,
            5,
            "Cycle count correction",
            SeedConstants.AdminUserId);

        var balance = await dbContext.InventoryBalances.SingleAsync(x => x.ItemId == SeedConstants.TaskChairItemId);
        var adjustment = await dbContext.StockAdjustments.SingleAsync();
        var transaction = await dbContext.InventoryTransactions.SingleAsync();
        var auditLog = await dbContext.AuditLogs.SingleAsync();

        Assert.Equal(23, balance.QuantityOnHand);
        Assert.Equal(AdjustmentType.Increase, adjustment.AdjustmentType);
        Assert.Equal("Cycle count correction", adjustment.Reason);
        Assert.Equal(InventoryTransactionType.AdjustmentIncrease, transaction.TransactionType);
        Assert.Equal(5, transaction.QuantityChange);
        Assert.Equal(SeedConstants.AdminUserId, adjustment.PerformedByUserId);
        Assert.Equal("StockAdjusted", auditLog.Action);
    }

    [Fact]
    public async Task AdjustStockAsync_Should_SupportNegativeAdjustments()
    {
        await using var dbContext = CreateDbContext();
        SeedBaseData(dbContext);

        var service = CreateService(dbContext);

        await service.AdjustStockAsync(
            SeedConstants.TaskChairItemId,
            SeedConstants.MainWarehouseId,
            SeedConstants.MainAisleLocationId,
            -4,
            "Damaged during transport",
            SeedConstants.AdminUserId);

        var balance = await dbContext.InventoryBalances.SingleAsync(x => x.ItemId == SeedConstants.TaskChairItemId);
        var adjustment = await dbContext.StockAdjustments.SingleAsync();
        var transaction = await dbContext.InventoryTransactions.SingleAsync();

        Assert.Equal(14, balance.QuantityOnHand);
        Assert.Equal(AdjustmentType.Decrease, adjustment.AdjustmentType);
        Assert.Equal(4, adjustment.QuantityDelta);
        Assert.Equal(InventoryTransactionType.AdjustmentDecrease, transaction.TransactionType);
        Assert.Equal(-4, transaction.QuantityChange);
    }

    [Fact]
    public async Task AdjustStockAsync_Should_Throw_WhenQuantityIsZero()
    {
        await using var dbContext = CreateDbContext();
        SeedBaseData(dbContext);

        var service = CreateService(dbContext);

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.AdjustStockAsync(
                SeedConstants.TaskChairItemId,
                SeedConstants.MainWarehouseId,
                SeedConstants.MainAisleLocationId,
                0,
                "No-op adjustment",
                SeedConstants.AdminUserId));

        Assert.Equal("Adjustment quantity must not be zero.", exception.Message);
    }

    private static InventoryService CreateService(ApplicationDbContext dbContext) =>
        new(
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

    private static void SeedBaseData(ApplicationDbContext dbContext)
    {
        dbContext.Users.Add(new User
        {
            Id = SeedConstants.AdminUserId,
            Email = "admin@clearerp.local",
            PasswordHash = "hash",
            FullName = "Admin"
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
            Name = "A1",
            Code = "A1"
        });

        dbContext.Items.Add(new Item
        {
            Id = SeedConstants.TaskChairItemId,
            CategoryId = SeedConstants.SeatingCategoryId,
            Sku = "CHR-1001",
            Name = "Task Chair",
            Unit = "EA",
            ReorderLevel = 10,
            StandardCost = 100m,
            IsActive = true
        });

        dbContext.InventoryBalances.Add(new InventoryBalance
        {
            Id = Guid.NewGuid(),
            ItemId = SeedConstants.TaskChairItemId,
            WarehouseId = SeedConstants.MainWarehouseId,
            LocationId = SeedConstants.MainAisleLocationId,
            QuantityOnHand = 18,
            QuantityReserved = 2,
            QuantityAvailable = 16
        });

        dbContext.SaveChanges();
    }
}

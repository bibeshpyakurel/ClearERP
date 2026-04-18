using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;
using ClearErp.Infrastructure.Persistence;
using ClearErp.Infrastructure.Persistence.Repositories;
using ClearErp.Infrastructure.Persistence.Seeding;
using ClearErp.Infrastructure.Services;

namespace ClearErp.UnitTests.Services;

public sealed class PurchaseOrderServiceTests
{
    [Fact]
    public async Task CreatePurchaseOrderAsync_Should_CreateDraftPurchaseOrder_WithCalculatedTotals_AndAuditLog()
    {
        await using var dbContext = CreateDbContext();
        SeedPurchaseOrderData(dbContext);

        var service = CreateService(dbContext);

        var purchaseOrder = await service.CreatePurchaseOrderAsync(
            "PO-AUDIT-1",
            SeedConstants.AcmeSupplierId,
            SeedConstants.AdminUserId,
            DateTime.UtcNow,
            null,
            new[]
            {
                new UpsertPurchaseOrderLineRequest(SeedConstants.TaskChairItemId, 2, 100m)
            });

        var auditLog = await dbContext.AuditLogs.SingleAsync();

        Assert.Equal(PurchaseOrderStatus.Draft, purchaseOrder.Status);
        Assert.Equal(200m, purchaseOrder.TotalAmount);
        Assert.Single(purchaseOrder.Lines);
        Assert.Equal("PurchaseOrderCreated", auditLog.Action);
        Assert.Equal(purchaseOrder.Id, auditLog.EntityId);
    }

    [Fact]
    public async Task ApprovePurchaseOrderAsync_Should_ChangeStatus_AndCreateAuditLog()
    {
        await using var dbContext = CreateDbContext();
        SeedPurchaseOrderData(dbContext);

        var service = CreateService(dbContext);
        var created = await service.CreatePurchaseOrderAsync(
            "PO-AUDIT-2",
            SeedConstants.AcmeSupplierId,
            SeedConstants.AdminUserId,
            DateTime.UtcNow,
            null,
            new[]
            {
                new UpsertPurchaseOrderLineRequest(SeedConstants.TaskChairItemId, 2, 100m)
            });

        var approved = await service.ApprovePurchaseOrderAsync(created.Id);

        var actions = await dbContext.AuditLogs.Select(x => x.Action).ToListAsync();

        Assert.Equal(PurchaseOrderStatus.Approved, approved.Status);
        Assert.Contains("PurchaseOrderCreated", actions);
        Assert.Contains("PurchaseOrderApproved", actions);
    }

    [Fact]
    public async Task UpdatePurchaseOrderAsync_Should_RecalculateTotal_AndCreateAuditLog()
    {
        await using var dbContext = CreateDbContext();
        SeedPurchaseOrderData(dbContext);

        var service = CreateService(dbContext);
        var created = await service.CreatePurchaseOrderAsync(
            "PO-AUDIT-3",
            SeedConstants.AcmeSupplierId,
            SeedConstants.AdminUserId,
            DateTime.UtcNow,
            null,
            new[]
            {
                new UpsertPurchaseOrderLineRequest(SeedConstants.TaskChairItemId, 2, 100m)
            });

        dbContext.ChangeTracker.Clear();

        var updated = await service.UpdatePurchaseOrderAsync(
            created.Id,
            SeedConstants.AcmeSupplierId,
            DateTime.UtcNow,
            null,
            new[]
            {
                new UpsertPurchaseOrderLineRequest(SeedConstants.TaskChairItemId, 3, 100m)
            });

        var actions = await dbContext.AuditLogs.Select(x => x.Action).ToListAsync();

        Assert.Equal(300m, updated.TotalAmount);
        Assert.Equal(3, updated.Lines.Single().OrderedQuantity);
        Assert.Contains("PurchaseOrderUpdated", actions);
    }

    [Fact]
    public async Task CreatePurchaseOrderAsync_Should_ThrowConflict_WhenPoNumberAlreadyExists()
    {
        await using var dbContext = CreateDbContext();
        SeedPurchaseOrderData(dbContext);

        var service = CreateService(dbContext);

        await service.CreatePurchaseOrderAsync(
            "PO-DUPLICATE",
            SeedConstants.AcmeSupplierId,
            SeedConstants.AdminUserId,
            DateTime.UtcNow,
            null,
            new[]
            {
                new UpsertPurchaseOrderLineRequest(SeedConstants.TaskChairItemId, 2, 100m)
            });

        var exception = await Assert.ThrowsAsync<ConflictException>(() =>
            service.CreatePurchaseOrderAsync(
                "PO-DUPLICATE",
                SeedConstants.AcmeSupplierId,
                SeedConstants.AdminUserId,
                DateTime.UtcNow,
                null,
                new[]
                {
                    new UpsertPurchaseOrderLineRequest(SeedConstants.TaskChairItemId, 1, 100m)
                }));

        Assert.Equal("PO number must be unique.", exception.Message);
    }

    private static PurchaseOrderService CreateService(ApplicationDbContext dbContext) =>
        new(
            new PurchaseOrderRepository(dbContext),
            new SupplierRepository(dbContext),
            new ItemRepository(dbContext),
            new AuditLogRepository(dbContext),
            dbContext);

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static void SeedPurchaseOrderData(ApplicationDbContext dbContext)
    {
        dbContext.Users.Add(new User
        {
            Id = SeedConstants.AdminUserId,
            Email = "admin@clearerp.local",
            PasswordHash = "hash",
            FullName = "Admin"
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

        dbContext.Items.Add(new Item
        {
            Id = SeedConstants.TaskChairItemId,
            CategoryId = SeedConstants.SeatingCategoryId,
            Sku = "CHR-1001",
            Name = "Task Chair",
            Unit = "EA",
            ReorderLevel = 2,
            StandardCost = 100m,
            IsActive = true
        });

        dbContext.SaveChanges();
    }
}

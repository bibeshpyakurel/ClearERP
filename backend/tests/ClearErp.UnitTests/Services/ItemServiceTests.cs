using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;
using ClearErp.Infrastructure.Persistence;
using ClearErp.Infrastructure.Persistence.Repositories;
using ClearErp.Infrastructure.Persistence.Seeding;
using ClearErp.Infrastructure.Services;

namespace ClearErp.UnitTests.Services;

public sealed class ItemServiceTests
{
    [Fact]
    public async Task CreateItemAsync_Should_CreateItem_AndCreateAuditLog()
    {
        await using var dbContext = CreateDbContext();
        dbContext.Categories.Add(new Category
        {
            Id = SeedConstants.SeatingCategoryId,
            Name = "Seating"
        });
        await dbContext.SaveChangesAsync();

        var service = new ItemService(
            new ItemRepository(dbContext),
            new CategoryRepository(dbContext),
            new AuditLogRepository(dbContext),
            new TestCurrentUserService(SeedConstants.AdminUserId),
            dbContext);

        var item = await service.CreateItemAsync(
            SeedConstants.SeatingCategoryId,
            "NEW-1001",
            "New Chair",
            "EA",
            5,
            45m,
            "Demo");

        var auditLog = await dbContext.AuditLogs.SingleAsync();

        Assert.Equal("NEW-1001", item.Sku);
        Assert.Equal("New Chair", item.Name);
        Assert.Equal(5, item.ReorderLevel);
        Assert.Equal("ItemCreated", auditLog.Action);
        Assert.Equal(item.Id, auditLog.EntityId);
    }

    [Fact]
    public async Task UpdateItemAsync_Should_CreateAuditLog()
    {
        await using var dbContext = CreateDbContext();
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
            ReorderLevel = 5,
            StandardCost = 50m,
            IsActive = true
        });
        await dbContext.SaveChangesAsync();

        var service = new ItemService(
            new ItemRepository(dbContext),
            new CategoryRepository(dbContext),
            new AuditLogRepository(dbContext),
            new TestCurrentUserService(SeedConstants.AdminUserId),
            dbContext);

        await service.UpdateItemAsync(
            SeedConstants.TaskChairItemId,
            SeedConstants.SeatingCategoryId,
            "CHR-1001",
            "Updated Chair",
            "EA",
            6,
            55m,
            "Updated");

        var auditLog = await dbContext.AuditLogs.SingleAsync();
        Assert.Equal("ItemUpdated", auditLog.Action);
    }

    [Fact]
    public async Task CreateItemAsync_Should_ThrowConflict_WhenSkuAlreadyExists()
    {
        await using var dbContext = CreateDbContext();
        SeedCategoryAndItems(dbContext);

        var service = CreateService(dbContext);

        var exception = await Assert.ThrowsAsync<ConflictException>(() =>
            service.CreateItemAsync(
                SeedConstants.SeatingCategoryId,
                "CHR-1001",
                "Duplicate Chair",
                "EA",
                4,
                40m,
                "Duplicate"));

        Assert.Equal("SKU must be unique.", exception.Message);
    }

    [Fact]
    public async Task SearchItemsAsync_Should_FilterBySearchAndActiveStatus()
    {
        await using var dbContext = CreateDbContext();
        SeedCategoryAndItems(dbContext);

        var service = CreateService(dbContext);

        var filtered = await service.SearchItemsAsync(new ItemFilter("chair", null, true));

        Assert.Single(filtered);
        Assert.Equal(SeedConstants.TaskChairItemId, filtered[0].Id);
    }

    [Fact]
    public async Task SetItemStatusAsync_Should_DeactivateItem_AndCreateAuditLog()
    {
        await using var dbContext = CreateDbContext();
        SeedCategoryAndItems(dbContext);

        var service = CreateService(dbContext);

        var item = await service.SetItemStatusAsync(SeedConstants.TaskChairItemId, false);

        var persistedItem = await dbContext.Items.SingleAsync(x => x.Id == SeedConstants.TaskChairItemId);
        var auditLog = await dbContext.AuditLogs.SingleAsync();

        Assert.False(item.IsActive);
        Assert.False(persistedItem.IsActive);
        Assert.Equal("ItemStatusChanged", auditLog.Action);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static ItemService CreateService(ApplicationDbContext dbContext) =>
        new(
            new ItemRepository(dbContext),
            new CategoryRepository(dbContext),
            new AuditLogRepository(dbContext),
            new TestCurrentUserService(SeedConstants.AdminUserId),
            dbContext);

    private static void SeedCategoryAndItems(ApplicationDbContext dbContext)
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
                ReorderLevel = 5,
                StandardCost = 50m,
                IsActive = true,
                Description = "Ergonomic task chair"
            },
            new Item
            {
                Id = SeedConstants.FilingCabinetItemId,
                CategoryId = SeedConstants.SeatingCategoryId,
                Sku = "CAB-1001",
                Name = "Filing Cabinet",
                Unit = "EA",
                ReorderLevel = 2,
                StandardCost = 120m,
                IsActive = false,
                Description = "Steel cabinet"
            });

        dbContext.SaveChanges();
    }

    private sealed class TestCurrentUserService(Guid userId) : ICurrentUserService
    {
        public Guid? UserId { get; } = userId;
        public string? Email => "admin@clearerp.local";
        public IReadOnlyCollection<string> Roles => ["Admin"];
        public bool IsAuthenticated => true;

        public bool IsInRole(string role) => Roles.Contains(role);
    }
}

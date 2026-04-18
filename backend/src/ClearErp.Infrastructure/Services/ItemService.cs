using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;

namespace ClearErp.Infrastructure.Services;

public sealed class ItemService(
    IItemRepository itemRepository,
    ICategoryRepository categoryRepository,
    IAuditLogRepository auditLogRepository,
    ICurrentUserService currentUserService,
    IApplicationDbContext dbContext) : IItemService
{
    public async Task<ItemDto> CreateItemAsync(
        Guid categoryId,
        string sku,
        string name,
        string unit,
        int reorderLevel,
        decimal standardCost,
        string? description,
        CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken)
            ?? throw new NotFoundException("Category was not found.");

        var existingItem = await itemRepository.GetBySkuAsync(sku, cancellationToken);
        if (existingItem is not null)
        {
            throw new ConflictException("SKU must be unique.");
        }

        var item = Item.Create(categoryId, sku, name, unit, reorderLevel, standardCost, description);
        var performedByUserId = GetPerformedByUserId();

        await itemRepository.AddAsync(item, cancellationToken);
        await auditLogRepository.AddAsync(
            AuditLog.Create("ItemCreated", nameof(Item), performedByUserId, $"Created item {item.Sku} - {item.Name}", item.Id),
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(item, category.Name);
    }

    public async Task<ItemDto?> GetItemByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await itemRepository.GetByIdAsync(id, cancellationToken);
        return item is null ? null : Map(item, item.Category?.Name ?? string.Empty);
    }

    public async Task<IReadOnlyList<ItemDto>> SearchItemsAsync(ItemFilter filter, CancellationToken cancellationToken = default)
    {
        var items = await itemRepository.SearchAsync(filter.Search, filter.CategoryId, filter.IsActive, cancellationToken);
        return items.Select(item => Map(item, item.Category?.Name ?? string.Empty)).ToArray();
    }

    public async Task<IReadOnlyList<ItemDto>> GetLowStockItemsAsync(CancellationToken cancellationToken = default)
    {
        var items = await itemRepository.GetLowStockAsync(cancellationToken);
        return items.Select(item => Map(item, item.Category?.Name ?? string.Empty)).ToArray();
    }

    public async Task<ItemDto> UpdateItemAsync(
        Guid id,
        Guid categoryId,
        string sku,
        string name,
        string unit,
        int reorderLevel,
        decimal standardCost,
        string? description,
        CancellationToken cancellationToken = default)
    {
        var item = await itemRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Item was not found.");

        var category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken)
            ?? throw new NotFoundException("Category was not found.");

        var existingItem = await itemRepository.GetBySkuAsync(sku, cancellationToken);
        if (existingItem is not null && existingItem.Id != item.Id)
        {
            throw new ConflictException("SKU must be unique.");
        }

        item.UpdateDetails(categoryId, sku, name, unit, reorderLevel, standardCost, description);
        var performedByUserId = GetPerformedByUserId();
        itemRepository.Update(item);
        await auditLogRepository.AddAsync(
            AuditLog.Create("ItemUpdated", nameof(Item), performedByUserId, $"Updated item {item.Sku} - {item.Name}", item.Id),
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(item, category.Name);
    }

    public async Task<ItemDto> SetItemStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default)
    {
        var item = await itemRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Item was not found.");

        if (isActive)
        {
            item.Activate();
        }
        else
        {
            item.Deactivate();
        }

        var performedByUserId = GetPerformedByUserId();
        itemRepository.Update(item);
        await auditLogRepository.AddAsync(
            AuditLog.Create("ItemStatusChanged", nameof(Item), performedByUserId, $"Set item {item.Sku} active={item.IsActive}", item.Id),
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(item, item.Category?.Name ?? string.Empty);
    }

    private static ItemDto Map(Item item, string categoryName) =>
        new(
            item.Id,
            item.CategoryId,
            categoryName,
            item.Sku,
            item.Name,
            item.Description,
            item.Unit,
            item.ReorderLevel,
            item.StandardCost,
            item.IsActive);

    private Guid GetPerformedByUserId() =>
        currentUserService.UserId
        ?? throw new UnauthorizedException("Authenticated user context is required for item changes.");
}

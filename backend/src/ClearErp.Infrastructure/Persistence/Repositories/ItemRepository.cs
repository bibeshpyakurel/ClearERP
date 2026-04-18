using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Domain.Entities;

namespace ClearErp.Infrastructure.Persistence.Repositories;

public sealed class ItemRepository(ApplicationDbContext dbContext) : IItemRepository
{
    public async Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Items
            .Include(x => x.Category)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Item?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        var normalized = sku.Trim().ToUpperInvariant();

        return await dbContext.Items
            .SingleOrDefaultAsync(x => x.Sku == normalized, cancellationToken);
    }

    public async Task<IReadOnlyList<Item>> SearchAsync(
        string? search,
        Guid? categoryId,
        bool? isActive,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Items
            .AsNoTracking()
            .Include(x => x.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.ToLower().Contains(term) ||
                x.Sku.ToLower().Contains(term) ||
                (x.Description != null && x.Description.ToLower().Contains(term)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == categoryId.Value);
        }

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Item>> GetLowStockAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Items
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(item => dbContext.InventoryBalances.Any(balance =>
                balance.ItemId == item.Id &&
                balance.QuantityAvailable <= item.ReorderLevel))
            .OrderBy(item => item.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Item item, CancellationToken cancellationToken = default)
    {
        await dbContext.Items.AddAsync(item, cancellationToken);
    }

    public void Update(Item item)
    {
        dbContext.Items.Update(item);
    }
}

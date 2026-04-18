using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Domain.Entities;

namespace ClearErp.Infrastructure.Persistence.Repositories;

public sealed class InventoryBalanceRepository(ApplicationDbContext dbContext) : IInventoryBalanceRepository
{
    public async Task<InventoryBalance?> GetByItemAndLocationAsync(Guid itemId, Guid warehouseId, Guid locationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.InventoryBalances
            .Include(x => x.Item)
            .Include(x => x.Warehouse)
            .Include(x => x.Location)
            .SingleOrDefaultAsync(
                x => x.ItemId == itemId && x.WarehouseId == warehouseId && x.LocationId == locationId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<InventoryBalance>> SearchAsync(Guid? itemId, Guid? warehouseId, CancellationToken cancellationToken = default)
    {
        var query = dbContext.InventoryBalances
            .AsNoTracking()
            .Include(x => x.Item)
            .Include(x => x.Warehouse)
            .Include(x => x.Location)
            .AsQueryable();

        if (itemId.HasValue)
        {
            query = query.Where(x => x.ItemId == itemId.Value);
        }

        if (warehouseId.HasValue)
        {
            query = query.Where(x => x.WarehouseId == warehouseId.Value);
        }

        return await query
            .OrderBy(x => x.Item!.Name)
            .ThenBy(x => x.Warehouse!.Name)
            .ThenBy(x => x.Location!.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(InventoryBalance inventoryBalance, CancellationToken cancellationToken = default)
    {
        await dbContext.InventoryBalances.AddAsync(inventoryBalance, cancellationToken);
    }

    public void Update(InventoryBalance inventoryBalance)
    {
        dbContext.InventoryBalances.Update(inventoryBalance);
    }
}

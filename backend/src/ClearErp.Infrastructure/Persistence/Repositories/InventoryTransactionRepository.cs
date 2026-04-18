using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;

namespace ClearErp.Infrastructure.Persistence.Repositories;

public sealed class InventoryTransactionRepository(ApplicationDbContext dbContext) : IInventoryTransactionRepository
{
    public async Task AddAsync(InventoryTransaction inventoryTransaction, CancellationToken cancellationToken = default)
    {
        await dbContext.InventoryTransactions.AddAsync(inventoryTransaction, cancellationToken);
    }

    public async Task<IReadOnlyList<InventoryTransaction>> SearchAsync(
        DateTime? fromDateUtc,
        DateTime? toDateUtc,
        Guid? itemId,
        Guid? warehouseId,
        InventoryTransactionType? transactionType,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.InventoryTransactions
            .AsNoTracking()
            .Include(x => x.Item)
            .Include(x => x.Warehouse)
            .Include(x => x.Location)
            .AsQueryable();

        if (fromDateUtc.HasValue)
        {
            query = query.Where(x => x.PerformedAt >= fromDateUtc.Value);
        }

        if (toDateUtc.HasValue)
        {
            query = query.Where(x => x.PerformedAt <= toDateUtc.Value);
        }

        if (itemId.HasValue)
        {
            query = query.Where(x => x.ItemId == itemId.Value);
        }

        if (warehouseId.HasValue)
        {
            query = query.Where(x => x.WarehouseId == warehouseId.Value);
        }

        if (transactionType.HasValue)
        {
            query = query.Where(x => x.TransactionType == transactionType.Value);
        }

        return await query
            .OrderByDescending(x => x.PerformedAt)
            .ToListAsync(cancellationToken);
    }
}

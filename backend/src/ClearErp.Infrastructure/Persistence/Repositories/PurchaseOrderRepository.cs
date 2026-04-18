using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;

namespace ClearErp.Infrastructure.Persistence.Repositories;

public sealed class PurchaseOrderRepository(ApplicationDbContext dbContext) : IPurchaseOrderRepository
{
    public async Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.PurchaseOrders
            .Include(x => x.Supplier)
            .Include(x => x.Lines)
            .ThenInclude(x => x.Item)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PurchaseOrder?> GetByNumberAsync(string poNumber, CancellationToken cancellationToken = default)
    {
        var normalized = poNumber.Trim().ToUpperInvariant();

        return await dbContext.PurchaseOrders
            .Include(x => x.Supplier)
            .Include(x => x.Lines)
            .ThenInclude(x => x.Item)
            .SingleOrDefaultAsync(x => x.PoNumber == normalized, cancellationToken);
    }

    public async Task<IReadOnlyList<PurchaseOrder>> SearchAsync(
        Guid? supplierId,
        PurchaseOrderStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.PurchaseOrders
            .AsNoTracking()
            .Include(x => x.Supplier)
            .Include(x => x.Lines)
            .ThenInclude(x => x.Item)
            .AsQueryable();

        if (supplierId.HasValue)
        {
            query = query.Where(x => x.SupplierId == supplierId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        return await query
            .OrderByDescending(x => x.OrderDate)
            .ThenBy(x => x.PoNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        await dbContext.PurchaseOrders.AddAsync(purchaseOrder, cancellationToken);
    }

    public void Update(PurchaseOrder purchaseOrder)
    {
        dbContext.PurchaseOrders.Update(purchaseOrder);
    }
}

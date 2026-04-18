using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Domain.Entities;

namespace ClearErp.Infrastructure.Persistence.Repositories;

public sealed class SupplierRepository(ApplicationDbContext dbContext) : ISupplierRepository
{
    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Suppliers
            .Include(x => x.SupplierItems)
            .ThenInclude(x => x.Item)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Supplier>> SearchAsync(string? search, bool? isActive, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Suppliers
            .AsNoTracking()
            .Include(x => x.SupplierItems)
            .ThenInclude(x => x.Item)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.ToLower().Contains(term) ||
                x.ContactName.ToLower().Contains(term) ||
                x.Email.ToLower().Contains(term) ||
                x.Phone.ToLower().Contains(term));
        }

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Supplier?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.Trim().ToLowerInvariant();

        return await dbContext.Suppliers
            .SingleOrDefaultAsync(x => x.Email.ToLower() == normalized, cancellationToken);
    }

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default)
    {
        await dbContext.Suppliers.AddAsync(supplier, cancellationToken);
    }

    public void Update(Supplier supplier)
    {
        dbContext.Suppliers.Update(supplier);
    }
}

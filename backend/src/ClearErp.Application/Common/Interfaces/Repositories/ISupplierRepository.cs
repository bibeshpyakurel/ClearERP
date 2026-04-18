using ClearErp.Domain.Entities;

namespace ClearErp.Application.Common.Interfaces.Repositories;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Supplier>> SearchAsync(string? search, bool? isActive, CancellationToken cancellationToken = default);
    Task<Supplier?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default);
    void Update(Supplier supplier);
}

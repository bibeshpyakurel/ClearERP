using ClearErp.Domain.Entities;

namespace ClearErp.Application.Common.Interfaces.Repositories;

public interface IItemRepository
{
    Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Item?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Item>> SearchAsync(string? search, Guid? categoryId, bool? isActive, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Item>> GetLowStockAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Item item, CancellationToken cancellationToken = default);
    void Update(Item item);
}

using ClearErp.Domain.Entities;

namespace ClearErp.Application.Common.Interfaces.Repositories;

public interface IInventoryBalanceRepository
{
    Task<InventoryBalance?> GetByItemAndLocationAsync(Guid itemId, Guid warehouseId, Guid locationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InventoryBalance>> SearchAsync(Guid? itemId, Guid? warehouseId, CancellationToken cancellationToken = default);
    Task AddAsync(InventoryBalance inventoryBalance, CancellationToken cancellationToken = default);
    void Update(InventoryBalance inventoryBalance);
}

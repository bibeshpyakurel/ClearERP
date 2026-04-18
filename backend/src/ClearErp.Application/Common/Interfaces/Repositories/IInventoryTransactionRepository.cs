using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;

namespace ClearErp.Application.Common.Interfaces.Repositories;

public interface IInventoryTransactionRepository
{
    Task AddAsync(InventoryTransaction inventoryTransaction, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InventoryTransaction>> SearchAsync(
        DateTime? fromDateUtc,
        DateTime? toDateUtc,
        Guid? itemId,
        Guid? warehouseId,
        InventoryTransactionType? transactionType,
        CancellationToken cancellationToken = default);
}

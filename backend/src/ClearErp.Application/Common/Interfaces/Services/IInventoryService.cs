namespace ClearErp.Application.Common.Interfaces.Services;

public interface IInventoryService
{
    Task<IReadOnlyList<InventoryBalanceDto>> GetBalancesAsync(
        InventoryBalanceFilter filter,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<InventoryTransactionDto>> GetTransactionsAsync(
        InventoryTransactionFilter filter,
        CancellationToken cancellationToken = default);

    Task ReceiveStockAsync(
        Guid itemId,
        Guid purchaseOrderId,
        Guid purchaseOrderLineId,
        Guid warehouseId,
        Guid locationId,
        int quantity,
        Guid performedByUserId,
        CancellationToken cancellationToken = default);

    Task IssueStockAsync(
        Guid itemId,
        Guid warehouseId,
        Guid locationId,
        int quantity,
        Guid performedByUserId,
        string referenceType,
        string reason,
        Guid? referenceId = null,
        CancellationToken cancellationToken = default);

    Task AdjustStockAsync(
        Guid itemId,
        Guid warehouseId,
        Guid locationId,
        int quantityDelta,
        string reason,
        Guid performedByUserId,
        Guid? referenceId = null,
        CancellationToken cancellationToken = default);
}

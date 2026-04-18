using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;

namespace ClearErp.Infrastructure.Services;

public sealed class InventoryService(
    IInventoryBalanceRepository inventoryBalanceRepository,
    IInventoryTransactionRepository inventoryTransactionRepository,
    IAuditLogRepository auditLogRepository,
    IApplicationDbContext dbContext) : IInventoryService
{
    public async Task<IReadOnlyList<InventoryBalanceDto>> GetBalancesAsync(
        InventoryBalanceFilter filter,
        CancellationToken cancellationToken = default)
    {
        var balances = await inventoryBalanceRepository.SearchAsync(filter.ItemId, filter.WarehouseId, cancellationToken);

        return balances.Select(balance => new InventoryBalanceDto(
            balance.Id,
            balance.ItemId,
            balance.Item?.Sku ?? string.Empty,
            balance.Item?.Name ?? string.Empty,
            balance.WarehouseId,
            balance.Warehouse?.Code ?? string.Empty,
            balance.Warehouse?.Name ?? string.Empty,
            balance.LocationId,
            balance.Location?.Code ?? string.Empty,
            balance.Location?.Name ?? string.Empty,
            balance.QuantityOnHand,
            balance.QuantityReserved,
            balance.QuantityAvailable)).ToArray();
    }

    public async Task<IReadOnlyList<InventoryTransactionDto>> GetTransactionsAsync(
        InventoryTransactionFilter filter,
        CancellationToken cancellationToken = default)
    {
        var transactions = await inventoryTransactionRepository.SearchAsync(
            filter.FromDateUtc,
            filter.ToDateUtc,
            filter.ItemId,
            filter.WarehouseId,
            filter.TransactionType,
            cancellationToken);

        return transactions.Select(transaction => new InventoryTransactionDto(
            transaction.Id,
            transaction.ItemId,
            transaction.Item?.Sku ?? string.Empty,
            transaction.Item?.Name ?? string.Empty,
            transaction.WarehouseId,
            transaction.Warehouse?.Code ?? string.Empty,
            transaction.Warehouse?.Name ?? string.Empty,
            transaction.LocationId,
            transaction.Location?.Code ?? string.Empty,
            transaction.Location?.Name ?? string.Empty,
            transaction.TransactionType,
            transaction.ReferenceType,
            transaction.ReferenceId,
            transaction.Reason,
            transaction.QuantityChange,
            transaction.BalanceAfter,
            transaction.PerformedByUserId,
            transaction.PerformedAt)).ToArray();
    }

    public async Task ReceiveStockAsync(
        Guid itemId,
        Guid purchaseOrderId,
        Guid purchaseOrderLineId,
        Guid warehouseId,
        Guid locationId,
        int quantity,
        Guid performedByUserId,
        CancellationToken cancellationToken = default)
    {
        await EnsureReferencesExistAsync(itemId, warehouseId, locationId, performedByUserId, cancellationToken);

        var balance = await inventoryBalanceRepository.GetByItemAndLocationAsync(itemId, warehouseId, locationId, cancellationToken)
            ?? InventoryBalance.Create(itemId, warehouseId, locationId);

        var wasNewBalance = balance.Id == default || await IsDetachedBalanceAsync(balance, cancellationToken);
        balance.Receive(quantity);

        if (wasNewBalance)
        {
            await inventoryBalanceRepository.AddAsync(balance, cancellationToken);
        }
        else
        {
            inventoryBalanceRepository.Update(balance);
        }

        var transaction = InventoryTransaction.Create(
            itemId,
            warehouseId,
            locationId,
            InventoryTransactionType.Receipt,
            "GoodsReceipt",
            null,
            quantity,
            balance.QuantityOnHand,
            performedByUserId,
            purchaseOrderLineId == Guid.Empty ? purchaseOrderId : purchaseOrderLineId);

        await inventoryTransactionRepository.AddAsync(transaction, cancellationToken);
        await auditLogRepository.AddAsync(
            AuditLog.Create("StockReceived", nameof(InventoryBalance), performedByUserId, $"Received {quantity} units for item {itemId}", itemId),
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task IssueStockAsync(
        Guid itemId,
        Guid warehouseId,
        Guid locationId,
        int quantity,
        Guid performedByUserId,
        string referenceType,
        string reason,
        Guid? referenceId = null,
        CancellationToken cancellationToken = default)
    {
        await EnsureReferencesExistAsync(itemId, warehouseId, locationId, performedByUserId, cancellationToken);
        Guard.AgainstNullOrWhiteSpace(reason, nameof(reason), 500);

        var balance = await inventoryBalanceRepository.GetByItemAndLocationAsync(itemId, warehouseId, locationId, cancellationToken)
            ?? throw new NotFoundException("Inventory balance was not found for the selected item and location.");

        balance.Issue(quantity);
        inventoryBalanceRepository.Update(balance);

        var transaction = InventoryTransaction.Create(
            itemId,
            warehouseId,
            locationId,
            InventoryTransactionType.Issue,
            referenceType,
            reason,
            -quantity,
            balance.QuantityOnHand,
            performedByUserId,
            referenceId);

        await inventoryTransactionRepository.AddAsync(transaction, cancellationToken);
        await auditLogRepository.AddAsync(
            AuditLog.Create("StockIssued", nameof(InventoryBalance), performedByUserId, $"Issued {quantity} units for item {itemId}. Reason: {reason}", itemId),
            cancellationToken);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new DomainException("The inventory balance was modified by another request. Please try again.");
        }
    }

    public async Task AdjustStockAsync(
        Guid itemId,
        Guid warehouseId,
        Guid locationId,
        int quantityDelta,
        string reason,
        Guid performedByUserId,
        Guid? referenceId = null,
        CancellationToken cancellationToken = default)
    {
        await EnsureReferencesExistAsync(itemId, warehouseId, locationId, performedByUserId, cancellationToken);

        if (quantityDelta == 0)
        {
            throw new DomainException("Adjustment quantity must not be zero.");
        }

        var balance = await inventoryBalanceRepository.GetByItemAndLocationAsync(itemId, warehouseId, locationId, cancellationToken)
            ?? InventoryBalance.Create(itemId, warehouseId, locationId);

        var wasNewBalance = balance.Id == default || await IsDetachedBalanceAsync(balance, cancellationToken);
        balance.Adjust(quantityDelta);

        if (wasNewBalance)
        {
            await inventoryBalanceRepository.AddAsync(balance, cancellationToken);
        }
        else
        {
            inventoryBalanceRepository.Update(balance);
        }

        var adjustmentType = quantityDelta > 0
            ? AdjustmentType.Increase
            : AdjustmentType.Decrease;

        var stockAdjustment = StockAdjustment.Create(
            itemId,
            warehouseId,
            locationId,
            adjustmentType,
            Math.Abs(quantityDelta),
            reason,
            performedByUserId);

        await dbContext.StockAdjustments.AddAsync(stockAdjustment, cancellationToken);

        var transactionType = quantityDelta > 0
            ? InventoryTransactionType.AdjustmentIncrease
            : InventoryTransactionType.AdjustmentDecrease;

        var transaction = InventoryTransaction.Create(
            itemId,
            warehouseId,
            locationId,
            transactionType,
            "StockAdjustment",
            reason,
            quantityDelta,
            balance.QuantityOnHand,
            performedByUserId,
            referenceId ?? stockAdjustment.Id);

        await inventoryTransactionRepository.AddAsync(transaction, cancellationToken);
        await auditLogRepository.AddAsync(
            AuditLog.Create("StockAdjusted", nameof(StockAdjustment), performedByUserId, $"Adjusted {quantityDelta} units for item {itemId}. Reason: {reason}", stockAdjustment.Id),
            cancellationToken);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new DomainException("The inventory balance was modified by another request. Please try again.");
        }
    }

    private async Task EnsureReferencesExistAsync(
        Guid itemId,
        Guid warehouseId,
        Guid locationId,
        Guid performedByUserId,
        CancellationToken cancellationToken)
    {
        var itemExists = await dbContext.Items.AnyAsync(x => x.Id == itemId, cancellationToken);
        var warehouseExists = await dbContext.Warehouses.AnyAsync(x => x.Id == warehouseId, cancellationToken);
        var locationExists = await dbContext.Locations.AnyAsync(x => x.Id == locationId, cancellationToken);
        var userExists = await dbContext.Users.AnyAsync(x => x.Id == performedByUserId, cancellationToken);

        if (!itemExists)
        {
            throw new NotFoundException("Item was not found.");
        }

        if (!warehouseExists)
        {
            throw new NotFoundException("Warehouse was not found.");
        }

        if (!locationExists)
        {
            throw new NotFoundException("Location was not found.");
        }

        if (!userExists)
        {
            throw new NotFoundException("User was not found.");
        }
    }

    private async Task<bool> IsDetachedBalanceAsync(InventoryBalance balance, CancellationToken cancellationToken)
    {
        return !await dbContext.InventoryBalances.AnyAsync(x => x.Id == balance.Id, cancellationToken);
    }
}

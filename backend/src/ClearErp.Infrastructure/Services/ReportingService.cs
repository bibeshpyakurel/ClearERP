using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Enums;

namespace ClearErp.Infrastructure.Services;

public sealed class ReportingService(IApplicationDbContext dbContext) : IReportingService
{
    public async Task<IReadOnlyList<LowStockReportItemDto>> GetLowStockReportAsync(CancellationToken cancellationToken = default)
    {
        var lowStockItems = await dbContext.InventoryBalances
            .AsNoTracking()
            .Include(balance => balance.Item)
            .Where(balance => balance.Item != null && balance.QuantityAvailable <= balance.Item.ReorderLevel)
            .OrderBy(balance => balance.Item!.Name)
            .Select(balance => new LowStockReportItemDto(
                balance.ItemId,
                balance.Item!.Sku,
                balance.Item.Name,
                balance.Item.ReorderLevel,
                balance.QuantityAvailable,
                balance.Item.ReorderLevel - balance.QuantityAvailable))
            .ToListAsync(cancellationToken);

        return lowStockItems;
    }

    public async Task<StockSummaryReportDto> GetStockSummaryAsync(CancellationToken cancellationToken = default)
    {
        var balances = await dbContext.InventoryBalances
            .AsNoTracking()
            .Include(balance => balance.Item)
            .ToListAsync(cancellationToken);

        var lowStockItemCount = balances
            .Where(balance => balance.Item != null && balance.QuantityAvailable <= balance.Item.ReorderLevel)
            .Select(balance => balance.ItemId)
            .Distinct()
            .Count();

        return new StockSummaryReportDto(
            balances.Select(x => x.ItemId).Distinct().Count(),
            balances.Sum(x => x.QuantityOnHand),
            balances.Sum(x => x.QuantityReserved),
            balances.Sum(x => x.QuantityAvailable),
            lowStockItemCount);
    }

    public async Task<StockValuationReportDto> GetStockValuationAsync(CancellationToken cancellationToken = default)
    {
        var balanceTotals = await dbContext.InventoryBalances
            .AsNoTracking()
            .GroupBy(balance => balance.ItemId)
            .Select(group => new
            {
                ItemId = group.Key,
                QuantityOnHand = group.Sum(x => x.QuantityOnHand)
            })
            .ToListAsync(cancellationToken);

        var itemIds = balanceTotals.Select(x => x.ItemId).ToArray();

        var itemLookup = await dbContext.Items
            .AsNoTracking()
            .Where(item => itemIds.Contains(item.Id))
            .Select(item => new
            {
                item.Id,
                item.Sku,
                item.Name,
                item.StandardCost
            })
            .ToDictionaryAsync(item => item.Id, cancellationToken);

        var items = balanceTotals
            .Where(total => itemLookup.ContainsKey(total.ItemId))
            .Select(total =>
            {
                var item = itemLookup[total.ItemId];
                return new StockValuationReportItemDto(
                    total.ItemId,
                    item.Sku,
                    item.Name,
                    total.QuantityOnHand,
                    item.StandardCost,
                    total.QuantityOnHand * item.StandardCost);
            })
            .OrderBy(x => x.ItemName)
            .ToList();

        return new StockValuationReportDto(
            items.Sum(x => x.InventoryValue),
            items);
    }

    public async Task<PurchaseOrderSummaryReportDto> GetPurchaseOrderSummaryAsync(CancellationToken cancellationToken = default)
    {
        var purchaseOrders = await dbContext.PurchaseOrders
            .AsNoTracking()
            .Include(po => po.Lines)
            .ToListAsync(cancellationToken);

        var openStatuses = new[] { PurchaseOrderStatus.Draft, PurchaseOrderStatus.Approved, PurchaseOrderStatus.PartiallyReceived };
        var totalOpenValue = purchaseOrders
            .Where(po => openStatuses.Contains(po.Status))
            .Sum(po => po.Lines.Sum(line => line.OrderedQuantity * line.UnitCost));

        return new PurchaseOrderSummaryReportDto(
            purchaseOrders.Count(po => po.Status == PurchaseOrderStatus.Draft),
            purchaseOrders.Count(po => po.Status == PurchaseOrderStatus.Approved),
            purchaseOrders.Count(po => po.Status == PurchaseOrderStatus.PartiallyReceived),
            purchaseOrders.Count(po => po.Status == PurchaseOrderStatus.Completed),
            purchaseOrders.Count(po => po.Status == PurchaseOrderStatus.Cancelled),
            totalOpenValue);
    }

    public async Task<IReadOnlyList<RecentTransactionReportItemDto>> GetRecentTransactionsAsync(int take = 10, CancellationToken cancellationToken = default)
    {
        var normalizedTake = Math.Clamp(take, 1, 100);

        var transactions = await dbContext.InventoryTransactions
            .AsNoTracking()
            .Include(transaction => transaction.Item)
            .OrderByDescending(transaction => transaction.PerformedAt)
            .Take(normalizedTake)
            .Select(transaction => new RecentTransactionReportItemDto(
                transaction.Id,
                transaction.ItemId,
                transaction.Item != null ? transaction.Item.Sku : string.Empty,
                transaction.Item != null ? transaction.Item.Name : string.Empty,
                transaction.TransactionType,
                transaction.QuantityChange,
                transaction.BalanceAfter,
                transaction.ReferenceType,
                transaction.Reason,
                transaction.PerformedAt))
            .ToListAsync(cancellationToken);

        return transactions;
    }
}

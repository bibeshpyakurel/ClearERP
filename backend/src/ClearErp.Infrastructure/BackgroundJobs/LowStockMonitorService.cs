using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ClearErp.Application.Common.Interfaces;

namespace ClearErp.Infrastructure.BackgroundJobs;

/// <summary>
/// Runs once every 24 hours and emits a structured warning log for every item
/// whose available quantity is at or below its configured reorder level.
/// Operators can route these warnings to alerting systems (Seq, Datadog, etc.)
/// using Serilog enrichers or sinks.
/// </summary>
public sealed class LowStockMonitorService(
    IServiceScopeFactory scopeFactory,
    ILogger<LowStockMonitorService> logger) : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(24);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Low stock monitor started. Check interval: {Interval}h", CheckInterval.TotalHours);

        // Run immediately on startup, then on the fixed interval.
        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCheckAsync(stoppingToken);

            try
            {
                await Task.Delay(CheckInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        logger.LogInformation("Low stock monitor stopped.");
    }

    private async Task RunCheckAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            var lowStockItems = await db.InventoryBalances
                .AsNoTracking()
                .Include(b => b.Item)
                .Where(b => b.Item != null && b.QuantityAvailable <= b.Item.ReorderLevel)
                .OrderBy(b => b.Item!.Name)
                .Select(b => new
                {
                    b.Item!.Sku,
                    b.Item.Name,
                    b.Item.ReorderLevel,
                    b.QuantityAvailable,
                    Shortfall = b.Item.ReorderLevel - b.QuantityAvailable
                })
                .ToListAsync(cancellationToken);

            if (lowStockItems.Count == 0)
            {
                logger.LogInformation("Low stock check complete — all items are adequately stocked.");
                return;
            }

            logger.LogWarning(
                "Low stock check found {Count} item(s) at or below reorder level.",
                lowStockItems.Count);

            foreach (var item in lowStockItems)
            {
                logger.LogWarning(
                    "LOW STOCK | SKU={Sku} Name={Name} Available={Available} ReorderLevel={ReorderLevel} Shortfall={Shortfall}",
                    item.Sku, item.Name, item.QuantityAvailable, item.ReorderLevel, item.Shortfall);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Low stock check failed.");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClearErp.Api.Contracts.Reports;
using ClearErp.Api.Middleware;
using ClearErp.Application.Common.Interfaces.Services;

namespace ClearErp.Api.Controllers;

[ApiController]
[Route("api/reports")]
[ApiExplorerSettings(GroupName = "Reports")]
[Produces("application/json")]
[Authorize]
public sealed class ReportsController(IReportingService reportingService) : ControllerBase
{
    /// <summary>
    /// Returns the current low-stock report for tracked items.
    /// </summary>
    [ProducesResponseType(typeof(IReadOnlyList<LowStockReportItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet("low-stock")]
    public async Task<ActionResult<IReadOnlyList<LowStockReportItemResponse>>> GetLowStock(CancellationToken cancellationToken)
    {
        var report = await reportingService.GetLowStockReportAsync(cancellationToken);
        return Ok(report.Select(item => new LowStockReportItemResponse(
            item.ItemId,
            item.ItemSku,
            item.ItemName,
            item.ReorderLevel,
            item.QuantityAvailable,
            item.Shortfall)));
    }

    /// <summary>
    /// Returns high-level inventory summary counts and quantities.
    /// </summary>
    [ProducesResponseType(typeof(StockSummaryReportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet("stock-summary")]
    public async Task<ActionResult<StockSummaryReportResponse>> GetStockSummary(CancellationToken cancellationToken)
    {
        var report = await reportingService.GetStockSummaryAsync(cancellationToken);
        return Ok(new StockSummaryReportResponse(
            report.TotalTrackedItems,
            report.TotalQuantityOnHand,
            report.TotalQuantityReserved,
            report.TotalQuantityAvailable,
            report.LowStockItemCount));
    }

    /// <summary>
    /// Returns inventory valuation totals and per-item values.
    /// </summary>
    [ProducesResponseType(typeof(StockValuationReportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet("stock-valuation")]
    public async Task<ActionResult<StockValuationReportResponse>> GetStockValuation(CancellationToken cancellationToken)
    {
        var report = await reportingService.GetStockValuationAsync(cancellationToken);
        return Ok(new StockValuationReportResponse(
            report.TotalInventoryValue,
            report.Items.Select(item => new StockValuationReportItemResponse(
                item.ItemId,
                item.ItemSku,
                item.ItemName,
                item.QuantityOnHand,
                item.StandardCost,
                item.InventoryValue)).ToArray()));
    }

    /// <summary>
    /// Returns purchase order counts by status and the total open PO value.
    /// </summary>
    [ProducesResponseType(typeof(PurchaseOrderSummaryReportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet("purchase-order-summary")]
    public async Task<ActionResult<PurchaseOrderSummaryReportResponse>> GetPurchaseOrderSummary(CancellationToken cancellationToken)
    {
        var report = await reportingService.GetPurchaseOrderSummaryAsync(cancellationToken);
        return Ok(new PurchaseOrderSummaryReportResponse(
            report.DraftCount,
            report.ApprovedCount,
            report.PartiallyReceivedCount,
            report.CompletedCount,
            report.CancelledCount,
            report.TotalOpenPurchaseOrderValue));
    }

    /// <summary>
    /// Returns the most recent inventory transactions for dashboard-style activity feeds.
    /// </summary>
    [ProducesResponseType(typeof(IReadOnlyList<RecentTransactionReportItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet("recent-transactions")]
    public async Task<ActionResult<IReadOnlyList<RecentTransactionReportItemResponse>>> GetRecentTransactions(
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default)
    {
        var report = await reportingService.GetRecentTransactionsAsync(take, cancellationToken);
        return Ok(report.Select(item => new RecentTransactionReportItemResponse(
            item.TransactionId,
            item.ItemId,
            item.ItemSku,
            item.ItemName,
            item.TransactionType.ToString(),
            item.QuantityChange,
            item.BalanceAfter,
            item.ReferenceType,
            item.Reason,
            item.PerformedAt)));
    }
}

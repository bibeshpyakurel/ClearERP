namespace ClearErp.Api.Contracts.Reports;

public sealed record StockSummaryReportResponse(
    int TotalTrackedItems,
    int TotalQuantityOnHand,
    int TotalQuantityReserved,
    int TotalQuantityAvailable,
    int LowStockItemCount);

namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record StockSummaryReportDto(
    int TotalTrackedItems,
    int TotalQuantityOnHand,
    int TotalQuantityReserved,
    int TotalQuantityAvailable,
    int LowStockItemCount);

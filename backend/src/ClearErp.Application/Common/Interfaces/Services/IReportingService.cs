namespace ClearErp.Application.Common.Interfaces.Services;

public interface IReportingService
{
    Task<IReadOnlyList<LowStockReportItemDto>> GetLowStockReportAsync(CancellationToken cancellationToken = default);
    Task<StockSummaryReportDto> GetStockSummaryAsync(CancellationToken cancellationToken = default);
    Task<StockValuationReportDto> GetStockValuationAsync(CancellationToken cancellationToken = default);
    Task<PurchaseOrderSummaryReportDto> GetPurchaseOrderSummaryAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RecentTransactionReportItemDto>> GetRecentTransactionsAsync(int take = 10, CancellationToken cancellationToken = default);
}

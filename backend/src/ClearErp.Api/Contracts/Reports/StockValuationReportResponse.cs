namespace ClearErp.Api.Contracts.Reports;

public sealed record StockValuationReportResponse(
    decimal TotalInventoryValue,
    IReadOnlyCollection<StockValuationReportItemResponse> Items);

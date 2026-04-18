namespace ClearErp.Api.Contracts.Reports;

public sealed record PurchaseOrderSummaryReportResponse(
    int DraftCount,
    int ApprovedCount,
    int PartiallyReceivedCount,
    int CompletedCount,
    int CancelledCount,
    decimal TotalOpenPurchaseOrderValue);

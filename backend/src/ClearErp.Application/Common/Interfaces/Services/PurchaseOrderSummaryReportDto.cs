namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record PurchaseOrderSummaryReportDto(
    int DraftCount,
    int ApprovedCount,
    int PartiallyReceivedCount,
    int CompletedCount,
    int CancelledCount,
    decimal TotalOpenPurchaseOrderValue);

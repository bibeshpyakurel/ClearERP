namespace ClearErp.Api.Contracts.Reports;

public sealed record RecentTransactionReportItemResponse(
    Guid TransactionId,
    Guid ItemId,
    string ItemSku,
    string ItemName,
    string TransactionType,
    int QuantityChange,
    int BalanceAfter,
    string ReferenceType,
    string? Reason,
    DateTime PerformedAt);

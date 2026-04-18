namespace ClearErp.Api.Contracts.Reports;

public sealed record LowStockReportItemResponse(
    Guid ItemId,
    string ItemSku,
    string ItemName,
    int ReorderLevel,
    int QuantityAvailable,
    int Shortfall);

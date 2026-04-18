namespace ClearErp.Api.Contracts.Reports;

public sealed record StockValuationReportItemResponse(
    Guid ItemId,
    string ItemSku,
    string ItemName,
    int QuantityOnHand,
    decimal StandardCost,
    decimal InventoryValue);

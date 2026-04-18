namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record StockValuationReportItemDto(
    Guid ItemId,
    string ItemSku,
    string ItemName,
    int QuantityOnHand,
    decimal StandardCost,
    decimal InventoryValue);

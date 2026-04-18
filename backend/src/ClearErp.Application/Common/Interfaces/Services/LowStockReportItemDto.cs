namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record LowStockReportItemDto(
    Guid ItemId,
    string ItemSku,
    string ItemName,
    int ReorderLevel,
    int QuantityAvailable,
    int Shortfall);

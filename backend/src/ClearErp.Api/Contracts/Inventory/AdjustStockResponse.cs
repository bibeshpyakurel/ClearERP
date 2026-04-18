namespace ClearErp.Api.Contracts.Inventory;

public sealed record AdjustStockResponse(
    string Message,
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    int QuantityDelta,
    Guid? ReferenceId,
    string Reason);

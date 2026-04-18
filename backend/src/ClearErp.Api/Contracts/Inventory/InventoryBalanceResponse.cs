namespace ClearErp.Api.Contracts.Inventory;

public sealed record InventoryBalanceResponse(
    Guid Id,
    Guid ItemId,
    string ItemSku,
    string ItemName,
    Guid WarehouseId,
    string WarehouseCode,
    string WarehouseName,
    Guid LocationId,
    string LocationCode,
    string LocationName,
    int QuantityOnHand,
    int QuantityReserved,
    int QuantityAvailable);

namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record InventoryBalanceDto(
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

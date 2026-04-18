namespace ClearErp.Api.Contracts.Inventory;

public sealed record InventoryTransactionResponse(
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
    string TransactionType,
    string ReferenceType,
    Guid? ReferenceId,
    string? Reason,
    int QuantityChange,
    int BalanceAfter,
    Guid PerformedByUserId,
    DateTime PerformedAt);

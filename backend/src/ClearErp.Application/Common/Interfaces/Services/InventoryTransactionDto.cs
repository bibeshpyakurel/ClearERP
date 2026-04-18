using ClearErp.Domain.Enums;

namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record InventoryTransactionDto(
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
    InventoryTransactionType TransactionType,
    string ReferenceType,
    Guid? ReferenceId,
    string? Reason,
    int QuantityChange,
    int BalanceAfter,
    Guid PerformedByUserId,
    DateTime PerformedAt);

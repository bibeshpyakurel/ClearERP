using ClearErp.Domain.Enums;

namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record InventoryTransactionFilter(
    DateTime? FromDateUtc,
    DateTime? ToDateUtc,
    Guid? ItemId,
    Guid? WarehouseId,
    InventoryTransactionType? TransactionType);

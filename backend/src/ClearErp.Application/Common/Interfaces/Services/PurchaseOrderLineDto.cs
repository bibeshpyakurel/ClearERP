namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record PurchaseOrderLineDto(
    Guid Id,
    Guid ItemId,
    string ItemSku,
    string ItemName,
    int OrderedQuantity,
    int ReceivedQuantity,
    decimal UnitCost,
    decimal LineTotal);

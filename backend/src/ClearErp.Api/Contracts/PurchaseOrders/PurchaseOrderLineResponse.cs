namespace ClearErp.Api.Contracts.PurchaseOrders;

public sealed record PurchaseOrderLineResponse(
    Guid Id,
    Guid ItemId,
    string ItemSku,
    string ItemName,
    int OrderedQuantity,
    int ReceivedQuantity,
    decimal UnitCost,
    decimal LineTotal);

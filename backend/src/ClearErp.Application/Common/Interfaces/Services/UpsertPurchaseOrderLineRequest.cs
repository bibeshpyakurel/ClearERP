namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record UpsertPurchaseOrderLineRequest(Guid ItemId, int OrderedQuantity, decimal UnitCost);

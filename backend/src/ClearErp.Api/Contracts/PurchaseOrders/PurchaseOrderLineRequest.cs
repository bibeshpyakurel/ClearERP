using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.PurchaseOrders;

public sealed record PurchaseOrderLineRequest(
    [property: Required] Guid ItemId,
    [property: Range(1, int.MaxValue)] int OrderedQuantity,
    [property: Range(typeof(decimal), "0", "79228162514264337593543950335")] decimal UnitCost);

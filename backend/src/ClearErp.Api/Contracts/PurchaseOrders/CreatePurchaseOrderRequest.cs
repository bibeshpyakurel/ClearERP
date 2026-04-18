using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.PurchaseOrders;

/// <summary>
/// Creates a draft purchase order.
/// Example payload:
/// {
///   "poNumber": "PO-2026-0001",
///   "supplierId": "11111111-1111-1111-1111-111111111111",
///   "createdByUserId": "22222222-2222-2222-2222-222222222222",
///   "orderDate": "2026-04-01T00:00:00Z",
///   "expectedDate": "2026-04-08T00:00:00Z",
///   "lines": [
///     {
///       "itemId": "33333333-3333-3333-3333-333333333333",
///       "orderedQuantity": 12,
///       "unitCost": 149.95
///     }
///   ]
/// }
/// </summary>
public sealed record CreatePurchaseOrderRequest(
    [property: Required, MaxLength(50)] string PoNumber,
    [property: Required] Guid SupplierId,
    [property: Required] DateTime OrderDate,
    DateTime? ExpectedDate,
    [property: MinLength(1)] IReadOnlyCollection<PurchaseOrderLineRequest> Lines);

using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.GoodsReceipts;

/// <summary>
/// Posts inventory receipt quantities against an existing purchase order.
/// Example payload:
/// {
///   "purchaseOrderId": "11111111-1111-1111-1111-111111111111",
///   "receiptNumber": "GR-1001",
///   "receivedByUserId": "22222222-2222-2222-2222-222222222222",
///   "receivedAtUtc": "2026-04-01T14:00:00Z",
///   "lines": [
///     {
///       "purchaseOrderLineId": "33333333-3333-3333-3333-333333333333",
///       "itemId": "44444444-4444-4444-4444-444444444444",
///       "receivedQuantity": 5,
///       "warehouseId": "55555555-5555-5555-5555-555555555555",
///       "locationId": "66666666-6666-6666-6666-666666666666"
///     }
///   ]
/// }
/// </summary>
public sealed record PostGoodsReceiptRequest(
    [property: Required] Guid PurchaseOrderId,
    [property: Required, MaxLength(50)] string ReceiptNumber,
    DateTime? ReceivedAtUtc,
    [property: MinLength(1)] IReadOnlyCollection<PostGoodsReceiptLineRequest> Lines);

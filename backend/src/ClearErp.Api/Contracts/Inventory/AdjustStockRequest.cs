using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.Inventory;

/// <summary>
/// Applies a manual positive or negative stock adjustment and records the reason.
/// Example payload:
/// {
///   "itemId": "11111111-1111-1111-1111-111111111111",
///   "warehouseId": "22222222-2222-2222-2222-222222222222",
///   "locationId": "33333333-3333-3333-3333-333333333333",
///   "performedByUserId": "44444444-4444-4444-4444-444444444444",
///   "quantityDelta": -1,
///   "referenceId": "55555555-5555-5555-5555-555555555555",
///   "reason": "Damaged during warehouse handling."
/// }
/// </summary>
public sealed record AdjustStockRequest(
    [property: Required] Guid ItemId,
    [property: Required] Guid WarehouseId,
    [property: Required] Guid LocationId,
    [property: Range(typeof(int), "-2147483648", "2147483647")] int QuantityDelta,
    Guid? ReferenceId,
    [property: Required, MaxLength(500)] string Reason);

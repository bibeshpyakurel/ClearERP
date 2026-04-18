using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.Inventory;

/// <summary>
/// Issues stock from inventory and records the business reference and reason.
/// Example payload:
/// {
///   "itemId": "11111111-1111-1111-1111-111111111111",
///   "warehouseId": "22222222-2222-2222-2222-222222222222",
///   "locationId": "33333333-3333-3333-3333-333333333333",
///   "quantity": 2,
///   "performedByUserId": "44444444-4444-4444-4444-444444444444",
///   "referenceType": "WorkOrder",
///   "referenceId": "55555555-5555-5555-5555-555555555555",
///   "reason": "Allocated chairs to installation work order."
/// }
/// </summary>
public sealed record IssueStockRequest(
    [property: Required] Guid ItemId,
    [property: Required] Guid WarehouseId,
    [property: Required] Guid LocationId,
    [property: Range(1, int.MaxValue)] int Quantity,
    [property: Required, MaxLength(50)] string ReferenceType,
    Guid? ReferenceId,
    [property: Required, MaxLength(500)] string Reason);

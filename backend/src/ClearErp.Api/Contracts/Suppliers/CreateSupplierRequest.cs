using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.Suppliers;

/// <summary>
/// Creates a supplier with optional preferred supplier-item mappings.
/// Example payload:
/// {
///   "name": "Northwood Seating Supply",
///   "contactName": "Maya Brooks",
///   "email": "maya.brooks@northwood.example",
///   "phone": "555-0100",
///   "notes": "Primary chair vendor for ergonomic seating lines.",
///   "items": [
///     {
///       "itemId": "11111111-1111-1111-1111-111111111111",
///       "supplierSku": "NW-CHR-2001"
///     }
///   ]
/// }
/// </summary>
public sealed record CreateSupplierRequest(
    [property: Required, MaxLength(200)] string Name,
    [property: Required, MaxLength(120)] string ContactName,
    [property: Required, EmailAddress, MaxLength(200)] string Email,
    [property: Required, MaxLength(30)] string Phone,
    [property: MaxLength(1000)] string? Notes,
    IReadOnlyCollection<SupplierItemRequest>? Items);

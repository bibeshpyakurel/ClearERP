using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.Items;

/// <summary>
/// Creates a new item in the ERP item master.
/// Example payload:
/// {
///   "categoryId": "11111111-1111-1111-1111-111111111111",
///   "sku": "CHR-2001",
///   "name": "Ergonomic Task Chair",
///   "description": "Adjustable office chair for open-plan workstations.",
///   "unit": "EA",
///   "reorderLevel": 10,
///   "standardCost": 149.95
/// }
/// </summary>
public sealed record CreateItemRequest(
    [property: Required] Guid CategoryId,
    [property: Required, MaxLength(64)] string Sku,
    [property: Required, MaxLength(200)] string Name,
    [property: MaxLength(1000)] string? Description,
    [property: Required, MaxLength(32)] string Unit,
    [property: Range(0, int.MaxValue)] int ReorderLevel,
    [property: Range(typeof(decimal), "0", "79228162514264337593543950335")] decimal StandardCost);

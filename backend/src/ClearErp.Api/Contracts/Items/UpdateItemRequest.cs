using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.Items;

public sealed record UpdateItemRequest(
    [property: Required] Guid CategoryId,
    [property: Required, MaxLength(64)] string Sku,
    [property: Required, MaxLength(200)] string Name,
    [property: MaxLength(1000)] string? Description,
    [property: Required, MaxLength(32)] string Unit,
    [property: Range(0, int.MaxValue)] int ReorderLevel,
    [property: Range(typeof(decimal), "0", "79228162514264337593543950335")] decimal StandardCost);

using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.Suppliers;

public sealed record SupplierItemRequest(
    [property: Required] Guid ItemId,
    [property: Required, MaxLength(64)] string SupplierSku);

using System.ComponentModel.DataAnnotations;

namespace ClearErp.Api.Contracts.Suppliers;

public sealed record UpdateSupplierRequest(
    [property: Required, MaxLength(200)] string Name,
    [property: Required, MaxLength(120)] string ContactName,
    [property: Required, EmailAddress, MaxLength(200)] string Email,
    [property: Required, MaxLength(30)] string Phone,
    [property: MaxLength(1000)] string? Notes,
    IReadOnlyCollection<SupplierItemRequest>? Items);

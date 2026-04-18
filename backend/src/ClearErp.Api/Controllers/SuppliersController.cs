using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClearErp.Api.Contracts.Suppliers;
using ClearErp.Api.Middleware;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Enums;

namespace ClearErp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Suppliers")]
[Produces("application/json")]
[Authorize]
public sealed class SuppliersController(ISupplierService supplierService) : ControllerBase
{
    /// <summary>
    /// Returns suppliers with optional search and active-status filters.
    /// </summary>
    [ProducesResponseType(typeof(IReadOnlyList<SupplierResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SupplierResponse>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken)
    {
        var suppliers = await supplierService.SearchSuppliersAsync(new SupplierFilter(search, isActive), cancellationToken);
        return Ok(suppliers.Select(Map));
    }

    /// <summary>
    /// Returns a supplier and its linked supplier-item records.
    /// </summary>
    [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SupplierResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var supplier = await supplierService.GetSupplierByIdAsync(id, cancellationToken);
        return supplier is null ? NotFound() : Ok(Map(supplier));
    }

    /// <summary>
    /// Creates a supplier record for procurement and replenishment workflows.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<SupplierResponse>> Create(CreateSupplierRequest request, CancellationToken cancellationToken)
    {
        var supplier = await supplierService.CreateSupplierAsync(
            request.Name,
            request.ContactName,
            request.Email,
            request.Phone,
            request.Notes,
            request.Items?.Select(x => new UpsertSupplierItemRequest(x.ItemId, x.SupplierSku)).ToArray()
                ?? Array.Empty<UpsertSupplierItemRequest>(),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, Map(supplier));
    }

    /// <summary>
    /// Updates supplier master data and optional supplier-item mappings.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SupplierResponse>> Update(Guid id, UpdateSupplierRequest request, CancellationToken cancellationToken)
    {
        var supplier = await supplierService.UpdateSupplierAsync(
            id,
            request.Name,
            request.ContactName,
            request.Email,
            request.Phone,
            request.Notes,
            request.Items?.Select(x => new UpsertSupplierItemRequest(x.ItemId, x.SupplierSku)).ToArray()
                ?? Array.Empty<UpsertSupplierItemRequest>(),
            cancellationToken);

        return Ok(Map(supplier));
    }

    /// <summary>
    /// Activates or deactivates a supplier without deleting historical purchasing data.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<SupplierResponse>> SetStatus(Guid id, SetSupplierStatusRequest request, CancellationToken cancellationToken)
    {
        var supplier = await supplierService.SetSupplierStatusAsync(id, request.IsActive, cancellationToken);
        return Ok(Map(supplier));
    }

    private static SupplierResponse Map(SupplierDto supplier) =>
        new(
            supplier.Id,
            supplier.Name,
            supplier.ContactName,
            supplier.Email,
            supplier.Phone,
            supplier.Notes,
            supplier.IsActive,
            supplier.Items.Select(x => new SupplierItemResponse(x.ItemId, x.ItemName, x.ItemSku, x.SupplierSku)).ToArray());
}

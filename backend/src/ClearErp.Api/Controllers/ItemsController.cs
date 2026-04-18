using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClearErp.Api.Contracts.Items;
using ClearErp.Api.Middleware;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Enums;

namespace ClearErp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Items")]
[Produces("application/json")]
[Authorize]
public sealed class ItemsController(IItemService itemService) : ControllerBase
{
    /// <summary>
    /// Returns items with optional search and category or active-status filters.
    /// </summary>
    [ProducesResponseType(typeof(IReadOnlyList<ItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ItemResponse>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] Guid? categoryId,
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken)
    {
        var items = await itemService.SearchItemsAsync(new ItemFilter(search, categoryId, isActive), cancellationToken);

        return Ok(items.Select(Map));
    }

    /// <summary>
    /// Returns a single item by identifier.
    /// </summary>
    [ProducesResponseType(typeof(ItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ItemResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await itemService.GetItemByIdAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(Map(item));
    }

    /// <summary>
    /// Creates a new inventory item in the item master catalog.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(ItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<ItemResponse>> Create(CreateItemRequest request, CancellationToken cancellationToken)
    {
        var item = await itemService.CreateItemAsync(
            request.CategoryId,
            request.Sku,
            request.Name,
            request.Unit,
            request.ReorderLevel,
            request.StandardCost,
            request.Description,
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, Map(item));
    }

    /// <summary>
    /// Updates the details of an existing item while preserving its history and current balances.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(ItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ItemResponse>> Update(Guid id, UpdateItemRequest request, CancellationToken cancellationToken)
    {
        var item = await itemService.UpdateItemAsync(
            id,
            request.CategoryId,
            request.Sku,
            request.Name,
            request.Unit,
            request.ReorderLevel,
            request.StandardCost,
            request.Description,
            cancellationToken);

        return Ok(Map(item));
    }

    /// <summary>
    /// Activates or deactivates an item without deleting it from the system.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(ItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<ItemResponse>> SetStatus(Guid id, SetItemStatusRequest request, CancellationToken cancellationToken)
    {
        var item = await itemService.SetItemStatusAsync(id, request.IsActive, cancellationToken);
        return Ok(Map(item));
    }

    /// <summary>
    /// Returns the subset of items whose available stock is at or below reorder level.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(IReadOnlyList<ItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [HttpGet("low-stock")]
    public async Task<ActionResult<IReadOnlyList<ItemResponse>>> GetLowStock(CancellationToken cancellationToken)
    {
        var items = await itemService.GetLowStockItemsAsync(cancellationToken);
        return Ok(items.Select(Map));
    }

    private static ItemResponse Map(ItemDto item) =>
        new(
            item.Id,
            item.CategoryId,
            item.CategoryName,
            item.Sku,
            item.Name,
            item.Description,
            item.Unit,
            item.ReorderLevel,
            item.StandardCost,
            item.IsActive);
}

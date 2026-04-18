using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClearErp.Api.Contracts.PurchaseOrders;
using ClearErp.Api.Middleware;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Enums;

namespace ClearErp.Api.Controllers;

[ApiController]
[Route("api/purchase-orders")]
[ApiExplorerSettings(GroupName = "Purchase Orders")]
[Produces("application/json")]
[Authorize]
public sealed class PurchaseOrdersController(
    IPurchaseOrderService purchaseOrderService,
    ICurrentUserService currentUserService) : ControllerBase
{
    /// <summary>
    /// Returns purchase orders filtered by supplier or workflow status.
    /// </summary>
    [ProducesResponseType(typeof(IReadOnlyList<PurchaseOrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PurchaseOrderResponse>>> GetAll(
        [FromQuery] Guid? supplierId,
        [FromQuery] PurchaseOrderStatus? status,
        CancellationToken cancellationToken)
    {
        var purchaseOrders = await purchaseOrderService.SearchPurchaseOrdersAsync(
            new PurchaseOrderFilter(supplierId, status),
            cancellationToken);

        return Ok(purchaseOrders.Select(Map));
    }

    /// <summary>
    /// Returns a single purchase order including line details and totals.
    /// </summary>
    [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PurchaseOrderResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var purchaseOrder = await purchaseOrderService.GetPurchaseOrderByIdAsync(id, cancellationToken);
        return purchaseOrder is null ? NotFound() : Ok(Map(purchaseOrder));
    }

    /// <summary>
    /// Creates a draft purchase order for future approval and receipt processing.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<PurchaseOrderResponse>> Create(CreatePurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await purchaseOrderService.CreatePurchaseOrderAsync(
            request.PoNumber,
            request.SupplierId,
            currentUserService.UserId!.Value,
            request.OrderDate,
            request.ExpectedDate,
            request.Lines.Select(x => new UpsertPurchaseOrderLineRequest(x.ItemId, x.OrderedQuantity, x.UnitCost)).ToArray(),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = purchaseOrder.Id }, Map(purchaseOrder));
    }

    /// <summary>
    /// Updates a draft purchase order before it has been approved.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PurchaseOrderResponse>> Update(Guid id, UpdatePurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await purchaseOrderService.UpdatePurchaseOrderAsync(
            id,
            request.SupplierId,
            request.OrderDate,
            request.ExpectedDate,
            request.Lines.Select(x => new UpsertPurchaseOrderLineRequest(x.ItemId, x.OrderedQuantity, x.UnitCost)).ToArray(),
            cancellationToken);

        return Ok(Map(purchaseOrder));
    }

    /// <summary>
    /// Changes purchase order workflow status. This endpoint currently supports approval and cancellation.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<PurchaseOrderResponse>> UpdateStatus(
        Guid id,
        UpdatePurchaseOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        PurchaseOrderDto purchaseOrder = request.Status switch
        {
            PurchaseOrderStatus.Approved => await purchaseOrderService.ApprovePurchaseOrderAsync(id, cancellationToken),
            PurchaseOrderStatus.Cancelled => await purchaseOrderService.CancelPurchaseOrderAsync(id, cancellationToken),
            _ => throw new ClearErp.Domain.Common.DomainException("Only Approved and Cancelled status transitions are supported by this endpoint.")
        };

        return Ok(Map(purchaseOrder));
    }

    private static PurchaseOrderResponse Map(PurchaseOrderDto purchaseOrder) =>
        new(
            purchaseOrder.Id,
            purchaseOrder.PoNumber,
            purchaseOrder.SupplierId,
            purchaseOrder.SupplierName,
            purchaseOrder.Status.ToString(),
            purchaseOrder.OrderDate,
            purchaseOrder.ExpectedDate,
            purchaseOrder.CreatedByUserId,
            purchaseOrder.TotalAmount,
            purchaseOrder.Lines.Select(line => new PurchaseOrderLineResponse(
                line.Id,
                line.ItemId,
                line.ItemSku,
                line.ItemName,
                line.OrderedQuantity,
                line.ReceivedQuantity,
                line.UnitCost,
                line.LineTotal)).ToArray());
}

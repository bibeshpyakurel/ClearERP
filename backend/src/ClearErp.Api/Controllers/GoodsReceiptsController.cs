using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClearErp.Api.Contracts.GoodsReceipts;
using ClearErp.Api.Middleware;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Enums;
using ServiceReceiptLineRequest = ClearErp.Application.Common.Interfaces.Services.PostGoodsReceiptLineRequest;

namespace ClearErp.Api.Controllers;

[ApiController]
[Route("api/goods-receipts")]
[ApiExplorerSettings(GroupName = "Goods Receipts")]
[Produces("application/json")]
[Authorize]
public sealed class GoodsReceiptsController(
    IGoodsReceiptService goodsReceiptService,
    ICurrentUserService currentUserService) : ControllerBase
{
    /// <summary>
    /// Receives inventory against an existing purchase order and updates stock balances and transaction history.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)},{nameof(RoleName.WarehouseStaff)}")]
    [ProducesResponseType(typeof(GoodsReceiptResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<GoodsReceiptResponse>> Create(
        PostGoodsReceiptRequest request,
        CancellationToken cancellationToken)
    {
        var receipt = await goodsReceiptService.ReceiveAgainstPurchaseOrderAsync(
            request.PurchaseOrderId,
            request.ReceiptNumber,
            currentUserService.UserId!.Value,
            request.ReceivedAtUtc,
            request.Lines.Select(line => new ServiceReceiptLineRequest(
                line.PurchaseOrderLineId,
                line.ItemId,
                line.ReceivedQuantity,
                line.WarehouseId,
                line.LocationId)).ToArray(),
            cancellationToken);

        return CreatedAtAction(nameof(Create), new { id = receipt.Id }, Map(receipt));
    }

    private static GoodsReceiptResponse Map(GoodsReceiptDto receipt) =>
        new(
            receipt.Id,
            receipt.PurchaseOrderId,
            receipt.ReceiptNumber,
            receipt.ReceivedAt,
            receipt.ReceivedByUserId,
            receipt.TotalReceivedAmount,
            receipt.Lines.Select(line => new GoodsReceiptLineResponse(
                line.Id,
                line.PurchaseOrderLineId,
                line.ItemId,
                line.ItemSku,
                line.ItemName,
                line.ReceivedQuantity)).ToArray());
}

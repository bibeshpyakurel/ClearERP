using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClearErp.Api.Contracts.Inventory;
using ClearErp.Api.Middleware;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Enums;

namespace ClearErp.Api.Controllers;

[ApiController]
[Route("api/inventory")]
[ApiExplorerSettings(GroupName = "Inventory")]
[Produces("application/json")]
[Authorize]
public sealed class InventoryController(
    IInventoryService inventoryService,
    ICurrentUserService currentUserService) : ControllerBase
{
    /// <summary>
    /// Returns inventory balances across warehouses and locations.
    /// </summary>
    [ProducesResponseType(typeof(IReadOnlyList<InventoryBalanceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet("balances")]
    public async Task<ActionResult<IReadOnlyList<InventoryBalanceResponse>>> GetBalances(
        [FromQuery] Guid? itemId,
        [FromQuery] Guid? warehouseId,
        CancellationToken cancellationToken)
    {
        var balances = await inventoryService.GetBalancesAsync(
            new InventoryBalanceFilter(itemId, warehouseId),
            cancellationToken);

        return Ok(balances.Select(x => new InventoryBalanceResponse(
            x.Id,
            x.ItemId,
            x.ItemSku,
            x.ItemName,
            x.WarehouseId,
            x.WarehouseCode,
            x.WarehouseName,
            x.LocationId,
            x.LocationCode,
            x.LocationName,
            x.QuantityOnHand,
            x.QuantityReserved,
            x.QuantityAvailable)));
    }

    /// <summary>
    /// Returns inventory transaction history with optional date, item, warehouse, and transaction-type filters.
    /// </summary>
    [ProducesResponseType(typeof(IReadOnlyList<InventoryTransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet("transactions")]
    public async Task<ActionResult<IReadOnlyList<InventoryTransactionResponse>>> GetTransactions(
        [FromQuery] DateTime? fromDateUtc,
        [FromQuery] DateTime? toDateUtc,
        [FromQuery] Guid? itemId,
        [FromQuery] Guid? warehouseId,
        [FromQuery] InventoryTransactionType? transactionType,
        CancellationToken cancellationToken)
    {
        var transactions = await inventoryService.GetTransactionsAsync(
            new InventoryTransactionFilter(fromDateUtc, toDateUtc, itemId, warehouseId, transactionType),
            cancellationToken);

        return Ok(transactions.Select(x => new InventoryTransactionResponse(
            x.Id,
            x.ItemId,
            x.ItemSku,
            x.ItemName,
            x.WarehouseId,
            x.WarehouseCode,
            x.WarehouseName,
            x.LocationId,
            x.LocationCode,
            x.LocationName,
            x.TransactionType.ToString(),
            x.ReferenceType,
            x.ReferenceId,
            x.Reason,
            x.QuantityChange,
            x.BalanceAfter,
            x.PerformedByUserId,
            x.PerformedAt)));
    }

    /// <summary>
    /// Issues stock from the selected item and location after validating available quantity.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)},{nameof(RoleName.WarehouseStaff)}")]
    [ProducesResponseType(typeof(IssueStockResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpPost("issues")]
    public async Task<ActionResult<IssueStockResponse>> IssueStock(
        IssueStockRequest request,
        CancellationToken cancellationToken)
    {
        await inventoryService.IssueStockAsync(
            request.ItemId,
            request.WarehouseId,
            request.LocationId,
            request.Quantity,
            currentUserService.UserId!.Value,
            request.ReferenceType,
            request.Reason,
            request.ReferenceId,
            cancellationToken);

        return Ok(new IssueStockResponse(
            "Stock issued successfully.",
            request.ItemId,
            request.WarehouseId,
            request.LocationId,
            request.Quantity,
            request.ReferenceType,
            request.ReferenceId,
            request.Reason));
    }

    /// <summary>
    /// Applies a manual inventory adjustment. Positive values increase stock and negative values decrease stock.
    /// </summary>
    [Authorize(Roles = $"{nameof(RoleName.Admin)},{nameof(RoleName.InventoryManager)}")]
    [ProducesResponseType(typeof(AdjustStockResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpPost("adjustments")]
    public async Task<ActionResult<AdjustStockResponse>> AdjustStock(
        AdjustStockRequest request,
        CancellationToken cancellationToken)
    {
        await inventoryService.AdjustStockAsync(
            request.ItemId,
            request.WarehouseId,
            request.LocationId,
            request.QuantityDelta,
            request.Reason,
            currentUserService.UserId!.Value,
            request.ReferenceId,
            cancellationToken);

        return Ok(new AdjustStockResponse(
            "Stock adjusted successfully.",
            request.ItemId,
            request.WarehouseId,
            request.LocationId,
            request.QuantityDelta,
            request.ReferenceId,
            request.Reason));
    }
}

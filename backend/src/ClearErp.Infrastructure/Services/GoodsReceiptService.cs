using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;

namespace ClearErp.Infrastructure.Services;

public sealed class GoodsReceiptService(
    IPurchaseOrderRepository purchaseOrderRepository,
    IInventoryBalanceRepository inventoryBalanceRepository,
    IInventoryTransactionRepository inventoryTransactionRepository,
    IAuditLogRepository auditLogRepository,
    IApplicationDbContext dbContext) : IGoodsReceiptService
{
    public async Task<GoodsReceiptDto> ReceiveAgainstPurchaseOrderAsync(
        Guid purchaseOrderId,
        string receiptNumber,
        Guid receivedByUserId,
        DateTime? receivedAtUtc,
        IReadOnlyCollection<PostGoodsReceiptLineRequest> lines,
        CancellationToken cancellationToken = default)
    {
        if (lines.Count == 0)
        {
            throw new DomainException("A goods receipt must contain at least one line.");
        }

        var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(purchaseOrderId, cancellationToken)
            ?? throw new NotFoundException("Purchase order was not found.");

        if (purchaseOrder.Status is PurchaseOrderStatus.Cancelled or PurchaseOrderStatus.Completed)
        {
            throw new DomainException("This purchase order is not receivable.");
        }

        var receiptNumberExists = await dbContext.GoodsReceipts.AnyAsync(
            x => x.ReceiptNumber == receiptNumber.Trim().ToUpperInvariant(),
            cancellationToken);

        if (receiptNumberExists)
        {
            throw new ConflictException("Receipt number must be unique.");
        }

        var userExists = await dbContext.Users.AnyAsync(x => x.Id == receivedByUserId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException("User was not found.");
        }

        var goodsReceipt = GoodsReceipt.Create(purchaseOrderId, receiptNumber, receivedByUserId, receivedAtUtc);

        foreach (var line in lines)
        {
            var purchaseOrderLine = purchaseOrder.Lines.SingleOrDefault(x => x.Id == line.PurchaseOrderLineId)
                ?? throw new NotFoundException("Purchase order line was not found.");

            if (purchaseOrderLine.ItemId != line.ItemId)
            {
                throw new DomainException("Receipt item does not match the purchase order line item.");
            }

            var itemExists = await dbContext.Items.AnyAsync(x => x.Id == line.ItemId, cancellationToken);
            var warehouseExists = await dbContext.Warehouses.AnyAsync(x => x.Id == line.WarehouseId, cancellationToken);
            var locationExists = await dbContext.Locations.AnyAsync(x => x.Id == line.LocationId, cancellationToken);

            if (!itemExists)
            {
                throw new NotFoundException("Item was not found.");
            }

            if (!warehouseExists)
            {
                throw new NotFoundException("Warehouse was not found.");
            }

            if (!locationExists)
            {
                throw new NotFoundException("Location was not found.");
            }

            purchaseOrder.RegisterReceipt(line.PurchaseOrderLineId, line.ReceivedQuantity);
            goodsReceipt.AddLine(line.PurchaseOrderLineId, line.ItemId, line.ReceivedQuantity);

            var balance = await inventoryBalanceRepository.GetByItemAndLocationAsync(
                line.ItemId,
                line.WarehouseId,
                line.LocationId,
                cancellationToken) ?? InventoryBalance.Create(line.ItemId, line.WarehouseId, line.LocationId);

            var balanceExists = await dbContext.InventoryBalances.AnyAsync(x => x.Id == balance.Id, cancellationToken);

            balance.Receive(line.ReceivedQuantity);

            if (balanceExists)
            {
                inventoryBalanceRepository.Update(balance);
            }
            else
            {
                await inventoryBalanceRepository.AddAsync(balance, cancellationToken);
            }

            var transaction = InventoryTransaction.Create(
                line.ItemId,
                line.WarehouseId,
                line.LocationId,
                InventoryTransactionType.Receipt,
                "GoodsReceipt",
                null,
                line.ReceivedQuantity,
                balance.QuantityOnHand,
                receivedByUserId,
                purchaseOrderLine.Id);

            await inventoryTransactionRepository.AddAsync(transaction, cancellationToken);
        }

        await dbContext.GoodsReceipts.AddAsync(goodsReceipt, cancellationToken);
        purchaseOrderRepository.Update(purchaseOrder);
        await auditLogRepository.AddAsync(
            AuditLog.Create("GoodsReceiptPosted", nameof(GoodsReceipt), receivedByUserId, $"Posted goods receipt {goodsReceipt.ReceiptNumber} for purchase order {purchaseOrder.PoNumber}", goodsReceipt.Id),
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(goodsReceipt, purchaseOrder);
    }

    private static GoodsReceiptDto Map(GoodsReceipt goodsReceipt, PurchaseOrder purchaseOrder)
    {
        var lineDetails = goodsReceipt.Lines.Select(line =>
        {
            var poLine = purchaseOrder.Lines.Single(x => x.Id == line.PurchaseOrderLineId);
            return new GoodsReceiptLineDto(
                line.Id,
                line.PurchaseOrderLineId,
                line.ItemId,
                poLine.Item?.Sku ?? string.Empty,
                poLine.Item?.Name ?? string.Empty,
                line.ReceivedQuantity);
        }).ToArray();

        var totalReceivedAmount = goodsReceipt.Lines.Sum(line =>
        {
            var poLine = purchaseOrder.Lines.Single(x => x.Id == line.PurchaseOrderLineId);
            return poLine.UnitCost * line.ReceivedQuantity;
        });

        return new GoodsReceiptDto(
            goodsReceipt.Id,
            goodsReceipt.PurchaseOrderId,
            goodsReceipt.ReceiptNumber,
            goodsReceipt.ReceivedAt,
            goodsReceipt.ReceivedByUserId,
            totalReceivedAmount,
            lineDetails);
    }
}

using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;

namespace ClearErp.Infrastructure.Services;

public sealed class PurchaseOrderService(
    IPurchaseOrderRepository purchaseOrderRepository,
    ISupplierRepository supplierRepository,
    IItemRepository itemRepository,
    IAuditLogRepository auditLogRepository,
    IApplicationDbContext dbContext) : IPurchaseOrderService
{
    public async Task<IReadOnlyList<PurchaseOrderDto>> SearchPurchaseOrdersAsync(
        PurchaseOrderFilter filter,
        CancellationToken cancellationToken = default)
    {
        var purchaseOrders = await purchaseOrderRepository.SearchAsync(filter.SupplierId, filter.Status, cancellationToken);
        return purchaseOrders.Select(Map).ToArray();
    }

    public async Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(id, cancellationToken);
        return purchaseOrder is null ? null : Map(purchaseOrder);
    }

    public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(
        string poNumber,
        Guid supplierId,
        Guid createdByUserId,
        DateTime orderDate,
        DateTime? expectedDate,
        IReadOnlyCollection<UpsertPurchaseOrderLineRequest> lines,
        CancellationToken cancellationToken = default)
    {
        await EnsureSupplierAndUserExistAsync(supplierId, createdByUserId, cancellationToken);
        await EnsureUniquePoNumberAsync(poNumber, null, cancellationToken);
        await ValidateLinesAsync(lines, cancellationToken);

        var purchaseOrder = PurchaseOrder.Create(poNumber, supplierId, createdByUserId, orderDate, expectedDate);
        purchaseOrder.ReplaceLines(lines.Select(x => (x.ItemId, x.OrderedQuantity, x.UnitCost)));

        await purchaseOrderRepository.AddAsync(purchaseOrder, cancellationToken);
        await auditLogRepository.AddAsync(
            AuditLog.Create("PurchaseOrderCreated", nameof(PurchaseOrder), createdByUserId, $"Created purchase order {purchaseOrder.PoNumber}", purchaseOrder.Id),
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var reloadedPurchaseOrder = await purchaseOrderRepository.GetByIdAsync(purchaseOrder.Id, cancellationToken)
            ?? purchaseOrder;

        return Map(reloadedPurchaseOrder);
    }

    public async Task<PurchaseOrderDto> UpdatePurchaseOrderAsync(
        Guid id,
        Guid supplierId,
        DateTime orderDate,
        DateTime? expectedDate,
        IReadOnlyCollection<UpsertPurchaseOrderLineRequest> lines,
        CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Purchase order was not found.");

        await EnsureSupplierAndUserExistAsync(supplierId, purchaseOrder.CreatedByUserId, cancellationToken);
        await ValidateLinesAsync(lines, cancellationToken);

        purchaseOrder.UpdateDraftDetails(supplierId, orderDate, expectedDate);
        purchaseOrder.ReplaceLines(lines.Select(x => (x.ItemId, x.OrderedQuantity, x.UnitCost)));

        purchaseOrderRepository.Update(purchaseOrder);
        await auditLogRepository.AddAsync(
            AuditLog.Create("PurchaseOrderUpdated", nameof(PurchaseOrder), purchaseOrder.CreatedByUserId, $"Updated purchase order {purchaseOrder.PoNumber}", purchaseOrder.Id),
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(purchaseOrder);
    }

    public async Task<PurchaseOrderDto> ApprovePurchaseOrderAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(purchaseOrderId, cancellationToken)
            ?? throw new NotFoundException("Purchase order was not found.");

        purchaseOrder.Approve();
        purchaseOrderRepository.Update(purchaseOrder);
        await auditLogRepository.AddAsync(
            AuditLog.Create("PurchaseOrderApproved", nameof(PurchaseOrder), purchaseOrder.CreatedByUserId, $"Approved purchase order {purchaseOrder.PoNumber}", purchaseOrder.Id),
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(purchaseOrder);
    }

    public async Task<PurchaseOrderDto> CancelPurchaseOrderAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(purchaseOrderId, cancellationToken)
            ?? throw new NotFoundException("Purchase order was not found.");

        purchaseOrder.Cancel();
        purchaseOrderRepository.Update(purchaseOrder);
        await auditLogRepository.AddAsync(
            AuditLog.Create("PurchaseOrderCancelled", nameof(PurchaseOrder), purchaseOrder.CreatedByUserId, $"Cancelled purchase order {purchaseOrder.PoNumber}", purchaseOrder.Id),
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(purchaseOrder);
    }

    private async Task EnsureSupplierAndUserExistAsync(Guid supplierId, Guid userId, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(supplierId, cancellationToken);
        if (supplier is null)
        {
            throw new NotFoundException("Supplier was not found.");
        }

        var userExists = await dbContext.Users.AnyAsync(x => x.Id == userId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException("User was not found.");
        }
    }

    private async Task EnsureUniquePoNumberAsync(string poNumber, Guid? currentPurchaseOrderId, CancellationToken cancellationToken)
    {
        var existingPurchaseOrder = await purchaseOrderRepository.GetByNumberAsync(poNumber, cancellationToken);
        if (existingPurchaseOrder is not null && existingPurchaseOrder.Id != currentPurchaseOrderId)
        {
            throw new ConflictException("PO number must be unique.");
        }
    }

    private async Task ValidateLinesAsync(
        IReadOnlyCollection<UpsertPurchaseOrderLineRequest> lines,
        CancellationToken cancellationToken)
    {
        if (lines.Count == 0)
        {
            throw new DomainException("A purchase order must have at least one line.");
        }

        if (lines.Select(x => x.ItemId).Distinct().Count() != lines.Count)
        {
            throw new DomainException("Each item may only appear once on a purchase order.");
        }

        foreach (var line in lines)
        {
            var item = await itemRepository.GetByIdAsync(line.ItemId, cancellationToken);
            if (item is null)
            {
                throw new NotFoundException($"Item {line.ItemId} was not found.");
            }
        }
    }

    private static PurchaseOrderDto Map(PurchaseOrder purchaseOrder) =>
        new(
            purchaseOrder.Id,
            purchaseOrder.PoNumber,
            purchaseOrder.SupplierId,
            purchaseOrder.Supplier?.Name ?? string.Empty,
            purchaseOrder.Status,
            purchaseOrder.OrderDate,
            purchaseOrder.ExpectedDate,
            purchaseOrder.CreatedByUserId,
            purchaseOrder.TotalAmount,
            purchaseOrder.Lines.Select(line => new PurchaseOrderLineDto(
                line.Id,
                line.ItemId,
                line.Item?.Sku ?? string.Empty,
                line.Item?.Name ?? string.Empty,
                line.OrderedQuantity,
                line.ReceivedQuantity,
                line.UnitCost,
                line.LineTotal)).ToArray());
}

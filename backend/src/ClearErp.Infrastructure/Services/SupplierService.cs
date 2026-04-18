using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Application.Common.Interfaces.Services;
using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;

namespace ClearErp.Infrastructure.Services;

public sealed class SupplierService(
    ISupplierRepository supplierRepository,
    IItemRepository itemRepository,
    IApplicationDbContext dbContext) : ISupplierService
{
    public async Task<IReadOnlyList<SupplierDto>> SearchSuppliersAsync(SupplierFilter filter, CancellationToken cancellationToken = default)
    {
        var suppliers = await supplierRepository.SearchAsync(filter.Search, filter.IsActive, cancellationToken);
        return suppliers.Select(Map).ToArray();
    }

    public async Task<SupplierDto?> GetSupplierByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var supplier = await supplierRepository.GetByIdAsync(id, cancellationToken);
        return supplier is null ? null : Map(supplier);
    }

    public async Task<SupplierDto> CreateSupplierAsync(
        string name,
        string contactName,
        string email,
        string phone,
        string? notes,
        IReadOnlyCollection<UpsertSupplierItemRequest> items,
        CancellationToken cancellationToken = default)
    {
        var existingSupplier = await supplierRepository.GetByEmailAsync(email, cancellationToken);
        if (existingSupplier is not null)
        {
            throw new ConflictException("Supplier email must be unique.");
        }

        var supplier = Supplier.Create(name, contactName, email, phone, notes);
        await SyncSupplierItemsAsync(supplier, items, cancellationToken);

        await supplierRepository.AddAsync(supplier, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(supplier);
    }

    public async Task<SupplierDto> UpdateSupplierAsync(
        Guid id,
        string name,
        string contactName,
        string email,
        string phone,
        string? notes,
        IReadOnlyCollection<UpsertSupplierItemRequest> items,
        CancellationToken cancellationToken = default)
    {
        var supplier = await supplierRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Supplier was not found.");

        var existingSupplier = await supplierRepository.GetByEmailAsync(email, cancellationToken);
        if (existingSupplier is not null && existingSupplier.Id != supplier.Id)
        {
            throw new ConflictException("Supplier email must be unique.");
        }

        supplier.UpdateDetails(name, contactName, email, phone, notes);
        supplier.SupplierItems.Clear();
        await SyncSupplierItemsAsync(supplier, items, cancellationToken);

        supplierRepository.Update(supplier);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(supplier);
    }

    public async Task<SupplierDto> SetSupplierStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default)
    {
        var supplier = await supplierRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Supplier was not found.");

        if (isActive)
        {
            supplier.Activate();
        }
        else
        {
            supplier.Deactivate();
        }

        supplierRepository.Update(supplier);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(supplier);
    }

    private async Task SyncSupplierItemsAsync(
        Supplier supplier,
        IReadOnlyCollection<UpsertSupplierItemRequest> items,
        CancellationToken cancellationToken)
    {
        foreach (var itemRequest in items)
        {
            var item = await itemRepository.GetByIdAsync(itemRequest.ItemId, cancellationToken)
                ?? throw new NotFoundException($"Item {itemRequest.ItemId} was not found.");

            supplier.SupplierItems.Add(SupplierItem.Create(supplier.Id, item.Id, itemRequest.SupplierSku));
        }
    }

    private static SupplierDto Map(Supplier supplier) =>
        new(
            supplier.Id,
            supplier.Name,
            supplier.ContactName,
            supplier.Email,
            supplier.Phone,
            supplier.Notes,
            supplier.IsActive,
            supplier.SupplierItems.Select(link => new SupplierItemDto(
                link.ItemId,
                link.Item?.Name ?? string.Empty,
                link.Item?.Sku ?? string.Empty,
                link.SupplierSku)).ToArray());
}

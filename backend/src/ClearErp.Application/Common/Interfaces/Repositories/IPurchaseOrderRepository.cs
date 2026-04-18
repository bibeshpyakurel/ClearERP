using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;

namespace ClearErp.Application.Common.Interfaces.Repositories;

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetByNumberAsync(string poNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PurchaseOrder>> SearchAsync(Guid? supplierId, PurchaseOrderStatus? status, CancellationToken cancellationToken = default);
    Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default);
    void Update(PurchaseOrder purchaseOrder);
}

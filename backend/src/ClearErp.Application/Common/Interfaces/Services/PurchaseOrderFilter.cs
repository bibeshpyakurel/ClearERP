using ClearErp.Domain.Enums;

namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record PurchaseOrderFilter(Guid? SupplierId, PurchaseOrderStatus? Status);

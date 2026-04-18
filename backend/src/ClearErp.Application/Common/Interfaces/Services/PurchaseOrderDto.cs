using ClearErp.Domain.Enums;

namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record PurchaseOrderDto(
    Guid Id,
    string PoNumber,
    Guid SupplierId,
    string SupplierName,
    PurchaseOrderStatus Status,
    DateTime OrderDate,
    DateTime? ExpectedDate,
    Guid CreatedByUserId,
    decimal TotalAmount,
    IReadOnlyCollection<PurchaseOrderLineDto> Lines);

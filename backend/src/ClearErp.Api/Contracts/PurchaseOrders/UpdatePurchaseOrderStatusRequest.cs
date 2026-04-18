using ClearErp.Domain.Enums;

namespace ClearErp.Api.Contracts.PurchaseOrders;

/// <summary>
/// Changes a purchase order status.
/// Example payload:
/// {
///   "status": "Approved"
/// }
/// </summary>
public sealed record UpdatePurchaseOrderStatusRequest(PurchaseOrderStatus Status);

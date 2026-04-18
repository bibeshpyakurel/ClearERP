namespace ClearErp.Api.Contracts.Inventory;

public sealed record IssueStockResponse(
    string Message,
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    int QuantityIssued,
    string ReferenceType,
    Guid? ReferenceId,
    string Reason);

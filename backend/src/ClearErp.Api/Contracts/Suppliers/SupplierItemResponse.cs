namespace ClearErp.Api.Contracts.Suppliers;

public sealed record SupplierItemResponse(
    Guid ItemId,
    string ItemName,
    string ItemSku,
    string SupplierSku);

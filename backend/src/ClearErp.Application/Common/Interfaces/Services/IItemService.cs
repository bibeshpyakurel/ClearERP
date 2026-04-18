namespace ClearErp.Application.Common.Interfaces.Services;

public interface IItemService
{
    Task<ItemDto> CreateItemAsync(
        Guid categoryId,
        string sku,
        string name,
        string unit,
        int reorderLevel,
        decimal standardCost,
        string? description,
        CancellationToken cancellationToken = default);

    Task<ItemDto?> GetItemByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ItemDto>> SearchItemsAsync(ItemFilter filter, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ItemDto>> GetLowStockItemsAsync(CancellationToken cancellationToken = default);
    Task<ItemDto> UpdateItemAsync(
        Guid id,
        Guid categoryId,
        string sku,
        string name,
        string unit,
        int reorderLevel,
        decimal standardCost,
        string? description,
        CancellationToken cancellationToken = default);
    Task<ItemDto> SetItemStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default);
}

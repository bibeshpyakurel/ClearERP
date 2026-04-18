namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record StockValuationReportDto(
    decimal TotalInventoryValue,
    IReadOnlyCollection<StockValuationReportItemDto> Items);

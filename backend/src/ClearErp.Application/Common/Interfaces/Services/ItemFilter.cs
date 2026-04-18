namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record ItemFilter(string? Search, Guid? CategoryId, bool? IsActive);

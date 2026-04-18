using ClearErp.Domain.Entities;

namespace ClearErp.Application.Common.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditLog>> GetRecentAsync(int take = 100, CancellationToken cancellationToken = default);
}

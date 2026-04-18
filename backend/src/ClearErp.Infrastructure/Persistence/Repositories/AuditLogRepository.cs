using Microsoft.EntityFrameworkCore;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Domain.Entities;

namespace ClearErp.Infrastructure.Persistence.Repositories;

public sealed class AuditLogRepository(ApplicationDbContext dbContext) : IAuditLogRepository
{
    public async Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default)
    {
        await dbContext.AuditLogs.AddAsync(auditLog, cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLog>> GetRecentAsync(int take = 100, CancellationToken cancellationToken = default)
    {
        return await dbContext.AuditLogs
            .AsNoTracking()
            .OrderByDescending(x => x.PerformedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}

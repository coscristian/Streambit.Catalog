using Microsoft.EntityFrameworkCore;
using Streambit.Application.Abstractions;
using Streambit.Domain.Aggregates.SyncAggregate;

namespace Streambit.Dal.Repositories;

public class SyncLogRepository : ISyncLogRepository
{
    private readonly DataContext _context;

    public SyncLogRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddAsync(SyncLog log, CancellationToken cancellationToken = default)
    {
        await _context.SyncLogs.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SyncLog>> ListAsync(
        string? entity,
        bool? succeeded,
        Guid? runId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var query = _context.SyncLogs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(entity))
            query = query.Where(log => log.Entity == entity);

        if (succeeded.HasValue)
            query = query.Where(log => log.Succeeded == succeeded.Value);

        if (runId.HasValue)
            query = query.Where(log => log.RunId == runId.Value);

        return await query
            .OrderByDescending(log => log.StartedAt)
            .ThenByDescending(log => log.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}

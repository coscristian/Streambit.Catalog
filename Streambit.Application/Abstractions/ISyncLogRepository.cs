using Streambit.Domain.Aggregates.SyncAggregate;

namespace Streambit.Application.Abstractions;

public interface ISyncLogRepository
{
    Task AddAsync(SyncLog log, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SyncLog>> ListAsync(
        string? entity,
        bool? succeeded,
        Guid? runId,
        int limit,
        CancellationToken cancellationToken = default);
}

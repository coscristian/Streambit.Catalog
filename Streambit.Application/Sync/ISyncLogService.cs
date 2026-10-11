using Streambit.Contracts.Sync;

namespace Streambit.Application.Sync;

public interface ISyncLogService
{
    Task<IReadOnlyList<SyncLogResponse>> ListAsync(
        string? entity,
        bool? succeeded,
        Guid? runId,
        int limit,
        CancellationToken cancellationToken = default);
}

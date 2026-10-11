using Streambit.Application.Abstractions;
using Streambit.Contracts.Sync;
using Streambit.Domain.Aggregates.SyncAggregate;

namespace Streambit.Application.Sync;

public class SyncLogService : ISyncLogService
{
    public const int DefaultLimit = 50;
    public const int MaxLimit = 100;

    private readonly ISyncLogRepository _logs;

    public SyncLogService(ISyncLogRepository logs)
    {
        _logs = logs;
    }

    public async Task<IReadOnlyList<SyncLogResponse>> ListAsync(
        string? entity,
        bool? succeeded,
        Guid? runId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var take = limit <= 0 ? DefaultLimit : Math.Min(limit, MaxLimit);
        var logs = await _logs.ListAsync(entity, succeeded, runId, take, cancellationToken);
        return logs.Select(Map).ToList();
    }

    private static SyncLogResponse Map(SyncLog log)
    {
        return new SyncLogResponse
        {
            Id = log.Id,
            RunId = log.RunId,
            Entity = log.Entity,
            Trigger = log.Trigger,
            Operation = log.Operation,
            Page = log.Page,
            Attempt = log.Attempt,
            HttpMethod = log.HttpMethod,
            RequestUrl = log.RequestUrl,
            HttpStatusCode = log.HttpStatusCode,
            Succeeded = log.Succeeded,
            ErrorMessage = log.ErrorMessage,
            ResponseBody = log.ResponseBody,
            StartedAt = log.StartedAt,
            FinishedAt = log.FinishedAt,
            DurationMs = log.DurationMs
        };
    }
}

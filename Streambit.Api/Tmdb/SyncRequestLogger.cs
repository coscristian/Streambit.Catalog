using Streambit.Application.Abstractions;
using Streambit.Application.Sync;
using Streambit.Domain.Aggregates.SyncAggregate;

namespace Streambit.Api.Tmdb;

public class SyncRequestLogger
{
    private readonly ISyncLogRepository _logs;
    private readonly ISyncLogContext _context;
    private readonly ILogger<SyncRequestLogger> _logger;

    public SyncRequestLogger(
        ISyncLogRepository logs,
        ISyncLogContext context,
        ILogger<SyncRequestLogger> logger)
    {
        _logs = logs;
        _context = context;
        _logger = logger;
    }

    public async Task WriteAsync(
        string operation,
        int? page,
        int attempt,
        string httpMethod,
        string requestUrl,
        int? httpStatusCode,
        bool succeeded,
        string? errorMessage,
        string? responseBody,
        DateTime startedAt,
        DateTime finishedAt,
        CancellationToken cancellationToken)
    {
        try
        {
            var log = SyncLog.Record(
                _context.RunId,
                _context.Entity,
                _context.Trigger,
                operation,
                page,
                attempt,
                httpMethod,
                requestUrl,
                httpStatusCode,
                succeeded,
                errorMessage,
                responseBody,
                startedAt,
                finishedAt);

            await _logs.AddAsync(log, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to persist sync log for {Operation}.", operation);
        }
    }
}

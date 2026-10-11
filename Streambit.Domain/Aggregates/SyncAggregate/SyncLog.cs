namespace Streambit.Domain.Aggregates.SyncAggregate;

public class SyncLog
{
    public int Id { get; private set; }
    public Guid RunId { get; private set; }
    public string Entity { get; private set; } = string.Empty;
    public string Trigger { get; private set; } = string.Empty;
    public string Operation { get; private set; } = string.Empty;
    public int? Page { get; private set; }
    public int Attempt { get; private set; }
    public string HttpMethod { get; private set; } = string.Empty;
    public string RequestUrl { get; private set; } = string.Empty;
    public int? HttpStatusCode { get; private set; }
    public bool Succeeded { get; private set; }
    public string ErrorMessage { get; private set; } = string.Empty;
    public string ResponseBody { get; private set; } = string.Empty;
    public DateTime StartedAt { get; private set; }
    public DateTime FinishedAt { get; private set; }
    public long DurationMs { get; private set; }

    private SyncLog() { }

    public static SyncLog Record(
        Guid runId,
        string entity,
        string trigger,
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
        DateTime finishedAt)
    {
        if (string.IsNullOrWhiteSpace(operation))
            throw new ArgumentException("Operation is required.", nameof(operation));

        if (attempt < 1)
            throw new ArgumentException("Attempt must be at least 1.", nameof(attempt));

        if (string.IsNullOrWhiteSpace(httpMethod))
            throw new ArgumentException("HTTP method is required.", nameof(httpMethod));

        if (string.IsNullOrWhiteSpace(requestUrl))
            throw new ArgumentException("Request URL is required.", nameof(requestUrl));

        var duration = finishedAt - startedAt;
        if (duration < TimeSpan.Zero)
            duration = TimeSpan.Zero;

        return new SyncLog
        {
            RunId = runId,
            Entity = entity?.Trim() ?? string.Empty,
            Trigger = trigger?.Trim() ?? string.Empty,
            Operation = operation.Trim(),
            Page = page,
            Attempt = attempt,
            HttpMethod = httpMethod.Trim(),
            RequestUrl = requestUrl.Trim(),
            HttpStatusCode = httpStatusCode,
            Succeeded = succeeded,
            ErrorMessage = errorMessage?.Trim() ?? string.Empty,
            ResponseBody = responseBody ?? string.Empty,
            StartedAt = startedAt,
            FinishedAt = finishedAt,
            DurationMs = (long)duration.TotalMilliseconds
        };
    }
}

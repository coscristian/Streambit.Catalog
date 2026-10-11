namespace Streambit.Contracts.Sync;

public class SyncLogResponse
{
    public int Id { get; set; }
    public Guid RunId { get; set; }
    public string Entity { get; set; } = string.Empty;
    public string Trigger { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public int? Page { get; set; }
    public int Attempt { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string RequestUrl { get; set; } = string.Empty;
    public int? HttpStatusCode { get; set; }
    public bool Succeeded { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string ResponseBody { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }
    public long DurationMs { get; set; }
}

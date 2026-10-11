using Streambit.Domain.Aggregates.SourceAggregate;

namespace Streambit.Domain.Aggregates.MovieAggregate;

public class MovieSource
{
    public int MovieId { get; private set; }
    public Movie Movie { get; private set; } = null!;

    public int SourceId { get; private set; }
    public Source Source { get; private set; } = null!;

    public string ExternalId { get; private set; } = string.Empty;

    private MovieSource() { }

    public static MovieSource Create(int sourceId, string externalId)
    {
        if (sourceId <= 0)
            throw new ArgumentException("SourceId is required.", nameof(sourceId));

        if (string.IsNullOrWhiteSpace(externalId))
            throw new ArgumentException("ExternalId is required.", nameof(externalId));

        return new MovieSource
        {
            SourceId = sourceId,
            ExternalId = externalId.Trim()
        };
    }

    public void ChangeExternalId(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            throw new ArgumentException("ExternalId is required.", nameof(externalId));

        ExternalId = externalId.Trim();
    }
}

using Streambit.Catalog.Domain.Aggregates.ProviderAggregate;

namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate;

public class MovieProvider
{
    public int MovieId  { get; set; }
    public Movie Movie { get; set; }
    
    public int ProviderId { get; set; }
    public Provider Provider { get; set; }
    
    public string ExternalId { get; set; } = string.Empty;
}
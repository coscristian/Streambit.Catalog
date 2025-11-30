using Streambit.Catalog.Domain.Aggregates.ProviderAggregate;

namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate;

public class MovieImage
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public ImageType Type { get; set; }
    
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public int ProviderId { get; set; }
    public Provider Provider { get; set; }
}

public enum ImageType
{
    Poster,
    Backdrop
}
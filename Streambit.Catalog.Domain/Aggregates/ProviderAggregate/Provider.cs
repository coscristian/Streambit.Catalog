using Streambit.Catalog.Domain.Aggregates.MovieAggregate;

namespace Streambit.Catalog.Domain.Aggregates.ProviderAggregate;

public class Provider
{
    public int Id { get; private set; }              
    public string Name { get; private set; }
    public ProviderCode Code { get; private set; }
    public ProviderType Type { get; private set; }
    
    public string? ApiBaseUrl { get; private set; }
    public string? WebBaseUrl { get; private set; }

    public bool SupportsRatings { get; private set; }
    public bool SupportsImages { get; private set; }
    public bool SupportsCast { get; private set; }
    public bool SupportsKeywords { get; private set; }
    
    private Provider() { }

    public Provider(int id, string name, ProviderCode code, ProviderType type)
    {
        Id = id;
        Name = name;
        Code = code;
        Type = type;
    }
}

public enum ProviderType
{
    Movies = 1,
    Tv = 2,
    Mixed = 3
}

public enum ProviderCode
{
    Tmdb,
    Imdb,
    Rt,
    Jw,
    Trakt
}
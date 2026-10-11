namespace Streambit.Contracts.People;

public class PersonKnownForResponse
{
    public int Id { get; set; }
    public int SortOrder { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public bool Adult { get; set; }
    public string BackdropPath { get; set; } = string.Empty;
    public string PosterPath { get; set; } = string.Empty;
    public List<int> GenreIds { get; set; } = [];
    public string OriginalLanguage { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public string OriginalName { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public string ReleaseDate { get; set; } = string.Empty;
    public string FirstAirDate { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Video { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public List<string> OriginCountries { get; set; } = [];
}

using Streambit.Contracts.Genres;

namespace Streambit.Contracts.Movies;

public class MovieResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Rating { get; set; }
    public string Overview { get; set; } = string.Empty;
    public int RuntimeMinutes { get; set; }
    public string PosterUrl { get; set; } = string.Empty;
    public string BackdropUrl { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public string Availability { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public string OriginalTitle { get; set; } = string.Empty;
    public string OriginalLanguage { get; set; } = string.Empty;
    public string ReleaseDate { get; set; } = string.Empty;
    public double Popularity { get; set; }
    public int VoteCount { get; set; }
    public bool Adult { get; set; }
    public bool Video { get; set; }
    public string PosterPath { get; set; } = string.Empty;
    public string BackdropPath { get; set; } = string.Empty;
    public List<MovieExternalIdResponse> ExternalIds { get; set; } = [];
    public List<GenreResponse> Genres { get; set; } = [];
    public List<MovieCreditResponse> Credits { get; set; } = [];
}

public class MovieExternalIdResponse
{
    public int SourceId { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
}

public class MovieCreditResponse
{
    public int PersonId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int BillingOrder { get; set; }
}

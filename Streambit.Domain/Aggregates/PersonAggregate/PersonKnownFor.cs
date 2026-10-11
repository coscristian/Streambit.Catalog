namespace Streambit.Domain.Aggregates.PersonAggregate;

public class PersonKnownFor
{
    public int Id { get; private set; }
    public int PersonId { get; private set; }
    public Person Person { get; private set; } = null!;

    public int SortOrder { get; private set; }
    public string ExternalId { get; private set; } = string.Empty;
    public string MediaType { get; private set; } = string.Empty;
    public bool Adult { get; private set; }
    public string BackdropPath { get; private set; } = string.Empty;
    public string PosterPath { get; private set; } = string.Empty;
    public string OriginalLanguage { get; private set; } = string.Empty;
    public string OriginalTitle { get; private set; } = string.Empty;
    public string OriginalName { get; private set; } = string.Empty;
    public string Overview { get; private set; } = string.Empty;
    public string ReleaseDate { get; private set; } = string.Empty;
    public string FirstAirDate { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public bool Video { get; private set; }
    public double VoteAverage { get; private set; }
    public int VoteCount { get; private set; }
    public List<int> GenreIds { get; private set; } = [];
    public List<string> OriginCountries { get; private set; } = [];

    private PersonKnownFor() { }

    public static PersonKnownFor Create(
        int sortOrder,
        string externalId,
        string mediaType,
        bool adult,
        string? backdropPath,
        string? posterPath,
        IReadOnlyList<int>? genreIds,
        string? originalLanguage,
        string? originalTitle,
        string? originalName,
        string? overview,
        string? releaseDate,
        string? firstAirDate,
        string? title,
        string? name,
        bool video,
        double voteAverage,
        int voteCount,
        IReadOnlyList<string>? originCountries)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            throw new ArgumentException("ExternalId is required.", nameof(externalId));

        if (string.IsNullOrWhiteSpace(mediaType))
            throw new ArgumentException("Media type is required.", nameof(mediaType));

        return new PersonKnownFor
        {
            SortOrder = sortOrder,
            ExternalId = externalId.Trim(),
            MediaType = mediaType.Trim(),
            Adult = adult,
            BackdropPath = backdropPath?.Trim() ?? string.Empty,
            PosterPath = posterPath?.Trim() ?? string.Empty,
            GenreIds = genreIds?.ToList() ?? [],
            OriginalLanguage = originalLanguage?.Trim() ?? string.Empty,
            OriginalTitle = originalTitle?.Trim() ?? string.Empty,
            OriginalName = originalName?.Trim() ?? string.Empty,
            Overview = overview?.Trim() ?? string.Empty,
            ReleaseDate = releaseDate?.Trim() ?? string.Empty,
            FirstAirDate = firstAirDate?.Trim() ?? string.Empty,
            Title = title?.Trim() ?? string.Empty,
            Name = name?.Trim() ?? string.Empty,
            Video = video,
            VoteAverage = voteAverage,
            VoteCount = voteCount,
            OriginCountries = originCountries?
                .Where(country => !string.IsNullOrWhiteSpace(country))
                .Select(country => country.Trim())
                .ToList() ?? []
        };
    }
}

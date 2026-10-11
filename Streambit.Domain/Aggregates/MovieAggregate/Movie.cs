namespace Streambit.Domain.Aggregates.MovieAggregate;

public class Movie
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public int Year { get; private set; }
    public decimal Rating { get; private set; }
    public string Overview { get; private set; } = string.Empty;
    public int RuntimeMinutes { get; private set; }
    public string PosterUrl { get; private set; } = string.Empty;
    public string BackdropUrl { get; private set; } = string.Empty;
    public ContentFormat Format { get; private set; }
    public Availability Availability { get; private set; }
    public bool IsFeatured { get; private set; }
    public string OriginalTitle { get; private set; } = string.Empty;
    public string OriginalLanguage { get; private set; } = string.Empty;
    public string ReleaseDate { get; private set; } = string.Empty;
    public double Popularity { get; private set; }
    public int VoteCount { get; private set; }
    public bool Adult { get; private set; }
    public bool Video { get; private set; }
    public string PosterPath { get; private set; } = string.Empty;
    public string BackdropPath { get; private set; } = string.Empty;
    public DateTime CreatedDate { get; private set; }
    public DateTime LastModified { get; private set; }

    private readonly List<MovieGenre> _movieGenres = [];
    public IReadOnlyCollection<MovieGenre> MovieGenres => _movieGenres;

    private readonly List<MovieSource> _movieSources = [];
    public IReadOnlyCollection<MovieSource> MovieSources => _movieSources;

    private readonly List<Credit> _credits = [];
    public IReadOnlyCollection<Credit> Credits => _credits;

    private Movie() { }

    public static Movie Create(
        string title,
        int year,
        string overview,
        int runtimeMinutes,
        string posterUrl,
        string backdropUrl,
        ContentFormat format,
        Availability availability,
        decimal rating = 0,
        bool isFeatured = false)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (year < 1888 || year > 2100)
            throw new ArgumentException("Year is out of range.", nameof(year));

        if (runtimeMinutes <= 0)
            throw new ArgumentException("Runtime must be greater than zero.", nameof(runtimeMinutes));

        if (string.IsNullOrWhiteSpace(posterUrl))
            throw new ArgumentException("Poster URL is required.", nameof(posterUrl));

        if (string.IsNullOrWhiteSpace(backdropUrl))
            throw new ArgumentException("Backdrop URL is required.", nameof(backdropUrl));

        if (!Enum.IsDefined(format))
            throw new ArgumentException("Format is not valid.", nameof(format));

        if (!Enum.IsDefined(availability))
            throw new ArgumentException("Availability is not valid.", nameof(availability));

        EnsureRating(rating);

        return new Movie
        {
            Title = title.Trim(),
            Year = year,
            Overview = overview?.Trim() ?? string.Empty,
            RuntimeMinutes = runtimeMinutes,
            PosterUrl = posterUrl.Trim(),
            BackdropUrl = backdropUrl.Trim(),
            Format = format,
            Availability = availability,
            Rating = rating,
            IsFeatured = isFeatured,
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    public static Movie Import(
        string title,
        string? originalTitle,
        string? originalLanguage,
        int year,
        string? releaseDate,
        string? overview,
        decimal rating,
        double popularity,
        int voteCount,
        bool adult,
        bool video,
        string? posterPath,
        string? posterUrl,
        string? backdropPath,
        string? backdropUrl,
        Availability availability)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (year < 0)
            throw new ArgumentException("Year cannot be negative.", nameof(year));

        EnsureRating(rating);

        if (double.IsNaN(popularity) || double.IsInfinity(popularity))
            throw new ArgumentException("Popularity is invalid.", nameof(popularity));

        if (voteCount < 0)
            throw new ArgumentException("Vote count cannot be negative.", nameof(voteCount));

        if (!Enum.IsDefined(availability))
            throw new ArgumentException("Availability is not valid.", nameof(availability));

        var overviewValue = overview?.Trim() ?? string.Empty;
        if (overviewValue.Length > 5000)
            overviewValue = overviewValue[..5000];

        return new Movie
        {
            Title = title.Trim(),
            OriginalTitle = originalTitle?.Trim() ?? string.Empty,
            OriginalLanguage = originalLanguage?.Trim() ?? string.Empty,
            Year = year,
            ReleaseDate = releaseDate?.Trim() ?? string.Empty,
            Overview = overviewValue,
            RuntimeMinutes = 0,
            Rating = rating,
            Popularity = popularity,
            VoteCount = voteCount,
            Adult = adult,
            Video = video,
            PosterPath = posterPath?.Trim() ?? string.Empty,
            PosterUrl = posterUrl?.Trim() ?? string.Empty,
            BackdropPath = backdropPath?.Trim() ?? string.Empty,
            BackdropUrl = backdropUrl?.Trim() ?? string.Empty,
            Format = ContentFormat.Movie,
            Availability = availability,
            IsFeatured = false,
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    public void AssignExternalId(int sourceId, string externalId)
    {
        var existing = _movieSources.FirstOrDefault(link => link.SourceId == sourceId);
        if (existing is null)
        {
            _movieSources.Add(MovieSource.Create(sourceId, externalId));
            return;
        }

        existing.ChangeExternalId(externalId);
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        var trimmed = title.Trim();
        if (trimmed == Title)
            return;

        Title = trimmed;
        MarkModified();
    }

    public void UpdateYear(int year)
    {
        if (year < 1888 || year > 2100)
            throw new ArgumentException("Year is out of range.", nameof(year));

        if (year == Year)
            return;

        Year = year;
        MarkModified();
    }

    public void UpdateOverview(string overview)
    {
        var value = overview?.Trim() ?? string.Empty;
        if (value == Overview)
            return;

        Overview = value;
        MarkModified();
    }

    public void UpdateRuntime(int runtimeMinutes)
    {
        if (runtimeMinutes <= 0)
            throw new ArgumentException("Runtime must be greater than zero.", nameof(runtimeMinutes));

        if (runtimeMinutes == RuntimeMinutes)
            return;

        RuntimeMinutes = runtimeMinutes;
        MarkModified();
    }

    public void UpdatePoster(string posterUrl)
    {
        if (string.IsNullOrWhiteSpace(posterUrl))
            throw new ArgumentException("Poster URL is required.", nameof(posterUrl));

        var trimmed = posterUrl.Trim();
        if (trimmed == PosterUrl)
            return;

        PosterUrl = trimmed;
        MarkModified();
    }

    public void UpdateBackdrop(string backdropUrl)
    {
        if (string.IsNullOrWhiteSpace(backdropUrl))
            throw new ArgumentException("Backdrop URL is required.", nameof(backdropUrl));

        var trimmed = backdropUrl.Trim();
        if (trimmed == BackdropUrl)
            return;

        BackdropUrl = trimmed;
        MarkModified();
    }

    public void UpdateRating(decimal rating)
    {
        EnsureRating(rating);
        if (rating == Rating)
            return;

        Rating = rating;
        MarkModified();
    }

    public void UpdateFormat(ContentFormat format)
    {
        if (!Enum.IsDefined(format))
            throw new ArgumentException("Format is not valid.", nameof(format));

        if (format == Format)
            return;

        Format = format;
        MarkModified();
    }

    public void UpdateAvailability(Availability availability)
    {
        if (!Enum.IsDefined(availability))
            throw new ArgumentException("Availability is not valid.", nameof(availability));

        if (availability == Availability)
            return;

        Availability = availability;
        MarkModified();
    }

    public void SetFeatured(bool isFeatured)
    {
        if (isFeatured == IsFeatured)
            return;

        IsFeatured = isFeatured;
        MarkModified();
    }

    public void AddGenre(int genreId)
    {
        if (_movieGenres.Any(genre => genre.GenreId == genreId))
            return;

        _movieGenres.Add(MovieGenre.Create(genreId));
        MarkModified();
    }

    public void RemoveGenre(int genreId)
    {
        var existing = _movieGenres.FirstOrDefault(genre => genre.GenreId == genreId);
        if (existing is null)
            return;

        _movieGenres.Remove(existing);
        MarkModified();
    }

    public void AddCredit(int personId, CreditRole role, int billingOrder)
    {
        if (_credits.Any(credit => credit.PersonId == personId))
            return;

        _credits.Add(Credit.Create(personId, role, billingOrder));
        MarkModified();
    }

    public void RemoveCredit(int personId)
    {
        var existing = _credits.FirstOrDefault(credit => credit.PersonId == personId);
        if (existing is null)
            return;

        _credits.Remove(existing);
        MarkModified();
    }

    private static void EnsureRating(decimal rating)
    {
        if (rating < 0 || rating > 10)
            throw new ArgumentException("Rating must be between 0 and 10.", nameof(rating));
    }

    private void MarkModified()
    {
        LastModified = DateTime.UtcNow;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate;

public class Movie
{
    public int Id { get; private set; }

    public string Title { get; private set; } = string.Empty;
    public string OriginalTitle { get; private set; } = string.Empty;
    public string OriginalLanguage { get; private set; } = string.Empty;
    public DateTime? ReleaseDate { get; private set; }
    public string Overview { get; private set; } = string.Empty;

    public bool IsAdult { get; private set; }
    public decimal Popularity { get; private set; }

    public decimal? Budget { get; private set; }
    public int? Revenue { get; private set; }
    public int? Runtime { get; private set; }
    public string TagLine { get; private set; } = string.Empty;
    public bool HasVideo { get; private set; }
    public decimal VoteAverage { get; private set; }
    public int VoteCount { get; private set; }

    public string BackdropPath { get; private set; } = string.Empty;

    public DateTime CreatedDate { get; private set; }
    public DateTime LastModified { get; private set; }

    // ========= NAVIGATION COLLECTIONS =========

    private readonly List<MovieGenre> _movieGenres = [];
    public IReadOnlyCollection<MovieGenre> MovieGenres => _movieGenres;

    private readonly List<MovieProvider> _movieProviders = [];
    public IReadOnlyCollection<MovieProvider> MovieProviders => _movieProviders;

    private readonly List<MovieImage> _movieImages = [];
    public IReadOnlyCollection<MovieImage> MovieImages => _movieImages;

    public static Movie Create(
        string title,
        string originalTitle,
        string originalLanguage,
        string overview,
        DateTime? releaseDate,
        bool isAdult,
        decimal popularity,
        string backdropPath)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required");

        var movie = new Movie
        {
            Title = title.Trim(),
            OriginalTitle = originalTitle?.Trim() ?? title.Trim(),
            OriginalLanguage = originalLanguage ?? "unknown",
            Overview = overview ?? string.Empty,
            ReleaseDate = releaseDate,
            IsAdult = isAdult,
            Popularity = popularity,
            BackdropPath = backdropPath ?? string.Empty,
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };

        return movie;
    }

    private void MarkModified()
    {
        LastModified = DateTime.UtcNow;
    }

    // ========= UPDATE METHODS =========

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            return;

        if (newTitle == Title)
            return;

        Title = newTitle.Trim();
        MarkModified();
    }

    public void UpdateOriginalTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return;

        OriginalTitle = title.Trim();
        MarkModified();
    }

    public void UpdateOriginalLanguage(string lang)
    {
        if (string.IsNullOrWhiteSpace(lang))
            return;

        OriginalLanguage = lang.Trim();
        MarkModified();
    }

    public void UpdateOverview(string newOverview)
    {
        if (newOverview is null || newOverview == Overview)
            return;

        Overview = newOverview;
        MarkModified();
    }

    public void UpdateReleaseDate(DateTime? newDate)
    {
        if (newDate == ReleaseDate)
            return;

        ReleaseDate = newDate;
        MarkModified();
    }

    public void UpdatePopularity(decimal popularity)
    {
        if (popularity < 0) return;
        if (popularity == Popularity) return;

        Popularity = popularity;
        MarkModified();
    }

    public void UpdateVoteAverage(decimal avg)
    {
        if (avg < 0 || avg > 10) return;
        if (avg == VoteAverage) return;

        VoteAverage = avg;
        MarkModified();
    }

    public void UpdateVoteCount(int count)
    {
        if (count < 0 || count == VoteCount) return;

        VoteCount = count;
        MarkModified();
    }

    public void UpdateBudget(decimal? budget)
    {
        if (budget < 0) return;

        Budget = budget;
        MarkModified();
    }

    public void UpdateRevenue(int? revenue)
    {
        if (revenue < 0) return;

        Revenue = revenue;
        MarkModified();
    }

    public void UpdateRuntime(int? runtime)
    {
        if (runtime < 0) return;

        Runtime = runtime;
        MarkModified();
    }

    public void UpdateTagLine(string tagLine)
    {
        TagLine = tagLine ?? string.Empty;
        MarkModified();
    }

    public void UpdateHasVideo(bool hasVideo)
    {
        if (HasVideo == hasVideo) return;

        HasVideo = hasVideo;
        MarkModified();
    }

    // ========= CHILD ENTITY OPERATIONS =========

    public void AddGenre(int genreId)
    {
        if (_movieGenres.Any(x => x.GenreId == genreId))
            return;

        _movieGenres.Add(MovieGenre.Create(Id, genreId));
        MarkModified();
    }

    public void RemoveGenre(int genreId)
    {
        var existing = _movieGenres.FirstOrDefault(g => g.GenreId == genreId);
        if (existing == null) return;

        _movieGenres.Remove(existing);
        MarkModified();
    }

    public void AddProvider(int providerId, string externalId)
    {
        if (_movieProviders.Any(x => x.ProviderId == providerId))
            return;

        _movieProviders.Add(new MovieProvider
        {
            MovieId = Id,
            ProviderId = providerId,
            ExternalId = externalId
        });

        MarkModified();
    }

    public void AddImage(int providerId, string url, ImageType type)
    {
        _movieImages.Add(new MovieImage
        {
            MovieId = Id,
            ProviderId = providerId,
            Url = url,
            Type = type
        });

        MarkModified();
    }
}
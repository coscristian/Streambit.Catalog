using System.Globalization;
using Streambit.Application.Abstractions;
using Streambit.Application.Genres;
using Streambit.Application.Sync;
using Streambit.Domain.Aggregates.MovieAggregate;
using Streambit.Domain.Aggregates.SourceAggregate;

namespace Streambit.Application.Movies;

public class MovieSyncService : IMovieSyncService
{
    public const int MaxPages = 500;

    private readonly ITmdbMovieClient _tmdb;
    private readonly IMovieRepository _movies;
    private readonly IGenreRepository _genres;
    private readonly ISourceRepository _sources;
    private readonly ISyncLogContext _syncLog;

    public MovieSyncService(
        ITmdbMovieClient tmdb,
        IMovieRepository movies,
        IGenreRepository genres,
        ISourceRepository sources,
        ISyncLogContext syncLog)
    {
        _tmdb = tmdb;
        _movies = movies;
        _genres = genres;
        _sources = sources;
        _syncLog = syncLog;
    }

    public async Task<MovieSyncResult> SyncAsync(string trigger, CancellationToken cancellationToken = default)
    {
        _syncLog.Begin(SyncEntity.Movie, trigger);

        var source = await EnsureSourceAsync(GenreSyncService.MovieSourceName, cancellationToken);
        var known = (await _movies.GetExternalIdsAsync(source.Id, cancellationToken))
            .ToHashSet(StringComparer.Ordinal);
        var genresByExternalId = await _genres.GetIdsByExternalIdAsync(source.Id, cancellationToken);

        var created = 0;
        var skipped = 0;
        var page = 1;
        var totalPages = 1;

        while (page <= totalPages)
        {
            var batch = await _tmdb.GetPopularMoviesAsync(page, cancellationToken);
            if (batch.TotalPages > 0)
                totalPages = Math.Min(batch.TotalPages, MaxPages);

            if (batch.Results.Count == 0)
                break;

            var pending = false;
            foreach (var item in batch.Results)
            {
                if (string.IsNullOrWhiteSpace(item.ExternalId) || string.IsNullOrWhiteSpace(item.Title))
                {
                    skipped++;
                    continue;
                }

                if (!known.Add(item.ExternalId))
                {
                    skipped++;
                    continue;
                }

                var movie = Movie.Import(
                    item.Title,
                    item.OriginalTitle,
                    item.OriginalLanguage,
                    ResolveYear(item.ReleaseDate),
                    item.ReleaseDate,
                    item.Overview,
                    ResolveRating(item.VoteAverage),
                    item.Popularity,
                    item.VoteCount,
                    item.Adult,
                    item.Video,
                    item.PosterPath,
                    item.PosterUrl,
                    item.BackdropPath,
                    item.BackdropUrl,
                    ResolveAvailability(item.ReleaseDate));

                movie.AssignExternalId(source.Id, item.ExternalId);
                foreach (var externalGenreId in item.GenreIds.Distinct())
                {
                    if (genresByExternalId.TryGetValue(externalGenreId.ToString(), out var genreId))
                        movie.AddGenre(genreId);
                }

                await _movies.AddAsync(movie, cancellationToken);
                created++;
                pending = true;
            }

            if (pending)
            {
                await _movies.SaveChangesAsync(cancellationToken);
                _movies.ClearChangeTracker();
            }

            page++;
        }

        return new MovieSyncResult(created, skipped);
    }

    private static int ResolveYear(string releaseDate)
    {
        return TryParseReleaseDate(releaseDate, out var date) ? date.Year : 0;
    }

    private static decimal ResolveRating(double voteAverage)
    {
        if (double.IsNaN(voteAverage) || voteAverage < 0)
            return 0;

        var rating = (decimal)Math.Round(voteAverage, 1, MidpointRounding.AwayFromZero);
        return rating > 10 ? 10 : rating;
    }

    private static Availability ResolveAvailability(string releaseDate)
    {
        if (TryParseReleaseDate(releaseDate, out var date) && date > DateOnly.FromDateTime(DateTime.UtcNow))
            return Availability.ComingSoon;

        return Availability.NowShowing;
    }

    private static bool TryParseReleaseDate(string releaseDate, out DateOnly date)
    {
        return DateOnly.TryParseExact(
            releaseDate,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out date);
    }

    private async Task<Source> EnsureSourceAsync(string name, CancellationToken cancellationToken)
    {
        var source = await _sources.GetByNameAsync(name, cancellationToken);
        if (source is not null)
            return source;

        source = Source.Create(name);
        await _sources.AddAsync(source, cancellationToken);
        await _sources.SaveChangesAsync(cancellationToken);
        return source;
    }
}

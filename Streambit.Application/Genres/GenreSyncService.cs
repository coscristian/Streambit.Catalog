using Streambit.Application.Abstractions;
using Streambit.Application.Sync;
using Streambit.Domain.Aggregates.GenreAggregate;
using Streambit.Domain.Aggregates.SourceAggregate;

namespace Streambit.Application.Genres;

public class GenreSyncService : IGenreSyncService
{
    public const string MovieSourceName = "TmdbMovies";
    public const string TvSourceName = "TmdbTv";

    private readonly ITmdbGenreClient _tmdb;
    private readonly IGenreRepository _genres;
    private readonly ISourceRepository _sources;
    private readonly ISyncLogContext _syncLog;

    public GenreSyncService(
        ITmdbGenreClient tmdb,
        IGenreRepository genres,
        ISourceRepository sources,
        ISyncLogContext syncLog)
    {
        _tmdb = tmdb;
        _genres = genres;
        _sources = sources;
        _syncLog = syncLog;
    }

    public async Task<GenreSyncResult> SyncAsync(string trigger, CancellationToken cancellationToken = default)
    {
        _syncLog.Begin(SyncEntity.Genre, trigger);

        var created = 0;
        var linked = 0;
        var skipped = 0;

        var movieGenres = await _tmdb.GetMovieGenresAsync(cancellationToken);
        (created, linked, skipped) = await SyncListAsync(MovieSourceName, movieGenres, created, linked, skipped, cancellationToken);

        var tvGenres = await _tmdb.GetTvGenresAsync(cancellationToken);
        (created, linked, skipped) = await SyncListAsync(TvSourceName, tvGenres, created, linked, skipped, cancellationToken);

        return new GenreSyncResult(created, linked, skipped);
    }

    private async Task<(int Created, int Linked, int Skipped)> SyncListAsync(
        string sourceName,
        IReadOnlyList<TmdbGenre> incoming,
        int created,
        int linked,
        int skipped,
        CancellationToken cancellationToken)
    {
        var source = await EnsureSourceAsync(sourceName, cancellationToken);

        foreach (var item in incoming)
        {
            if (string.IsNullOrWhiteSpace(item.ExternalId) || string.IsNullOrWhiteSpace(item.Name))
            {
                skipped++;
                continue;
            }

            var alreadyLinked = await _genres.GetByExternalIdAsync(source.Id, item.ExternalId, cancellationToken);
            if (alreadyLinked is not null)
            {
                skipped++;
                continue;
            }

            var existing = await _genres.GetByNameAsync(item.Name, cancellationToken);
            if (existing is null)
            {
                existing = Genre.Create(item.Name);
                existing.AssignExternalId(source.Id, item.ExternalId);
                await _genres.AddAsync(existing, cancellationToken);
                await _genres.SaveChangesAsync(cancellationToken);
                created++;
                continue;
            }

            existing.AssignExternalId(source.Id, item.ExternalId);
            await _genres.SaveChangesAsync(cancellationToken);
            linked++;
        }

        return (created, linked, skipped);
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

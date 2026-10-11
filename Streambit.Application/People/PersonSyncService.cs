using Streambit.Application.Abstractions;
using Streambit.Application.Sync;
using Streambit.Domain.Aggregates.PersonAggregate;
using Streambit.Domain.Aggregates.SourceAggregate;

namespace Streambit.Application.People;

public class PersonSyncService : IPersonSyncService
{
    public const string SourceName = "TmdbPeople";

    private readonly ITmdbPersonClient _tmdb;
    private readonly IPersonRepository _people;
    private readonly ISourceRepository _sources;
    private readonly ISyncLogContext _syncLog;

    public PersonSyncService(
        ITmdbPersonClient tmdb,
        IPersonRepository people,
        ISourceRepository sources,
        ISyncLogContext syncLog)
    {
        _tmdb = tmdb;
        _people = people;
        _sources = sources;
        _syncLog = syncLog;
    }

    public async Task<PersonSyncResult> SyncAsync(string trigger, CancellationToken cancellationToken = default)
    {
        _syncLog.Begin(SyncEntity.Person, trigger);

        var source = await EnsureSourceAsync(SourceName, cancellationToken);
        var known = (await _people.GetExternalIdsAsync(source.Id, cancellationToken))
            .ToHashSet(StringComparer.Ordinal);

        var created = 0;
        var skipped = 0;
        var page = 1;
        var totalPages = 1;

        while (page <= totalPages)
        {
            var batch = await _tmdb.GetPopularPeopleAsync(page, cancellationToken);
            if (batch.TotalPages > 0)
                totalPages = batch.TotalPages;

            if (batch.Results.Count == 0)
                break;

            var pending = false;
            foreach (var item in batch.Results)
            {
                if (string.IsNullOrWhiteSpace(item.ExternalId) || string.IsNullOrWhiteSpace(item.Name))
                {
                    skipped++;
                    continue;
                }

                if (!known.Add(item.ExternalId))
                {
                    skipped++;
                    continue;
                }

                var person = Person.Import(
                    item.Name,
                    item.PhotoUrl,
                    item.ProfilePath,
                    item.Adult,
                    item.Gender,
                    item.KnownForDepartment,
                    item.Popularity);

                person.AssignExternalId(source.Id, item.ExternalId);
                AddKnownFor(person, item.KnownFor);
                await _people.AddAsync(person, cancellationToken);
                created++;
                pending = true;
            }

            if (pending)
            {
                await _people.SaveChangesAsync(cancellationToken);
                _people.ClearChangeTracker();
            }

            page++;
        }

        return new PersonSyncResult(created, skipped);
    }

    private static void AddKnownFor(Person person, IReadOnlyList<TmdbKnownFor> works)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var sortOrder = 0;

        foreach (var work in works)
        {
            if (string.IsNullOrWhiteSpace(work.ExternalId) || string.IsNullOrWhiteSpace(work.MediaType))
                continue;

            if (!seen.Add($"{work.MediaType}:{work.ExternalId}"))
                continue;

            person.AddKnownFor(PersonKnownFor.Create(
                sortOrder,
                work.ExternalId,
                work.MediaType,
                work.Adult,
                work.BackdropPath,
                work.PosterPath,
                work.GenreIds,
                work.OriginalLanguage,
                work.OriginalTitle,
                work.OriginalName,
                work.Overview,
                work.ReleaseDate,
                work.FirstAirDate,
                work.Title,
                work.Name,
                work.Video,
                work.VoteAverage,
                work.VoteCount,
                work.OriginCountries));

            sortOrder++;
        }
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

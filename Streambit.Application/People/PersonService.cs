using Streambit.Application.Abstractions;
using Streambit.Contracts.People;
using Streambit.Domain.Aggregates.PersonAggregate;

namespace Streambit.Application.People;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _people;

    public PersonService(IPersonRepository people)
    {
        _people = people;
    }

    public async Task<IReadOnlyList<PersonResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var people = await _people.GetAllAsync(cancellationToken);
        return people.Select(Map).ToList();
    }

    public async Task<PersonResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var person = await _people.GetByIdAsync(id, cancellationToken);
        return person is null ? null : Map(person);
    }

    public async Task<PersonResponse> CreateAsync(PersonRequest request, CancellationToken cancellationToken = default)
    {
        var person = Person.Create(request.Name, request.PhotoUrl);
        await _people.AddAsync(person, cancellationToken);
        await _people.SaveChangesAsync(cancellationToken);
        return Map(person);
    }

    public async Task<PersonResponse?> UpdateAsync(int id, PersonRequest request, CancellationToken cancellationToken = default)
    {
        var person = await _people.GetByIdAsync(id, cancellationToken);
        if (person is null)
            return null;

        person.UpdateName(request.Name);
        person.UpdatePhoto(request.PhotoUrl);
        await _people.SaveChangesAsync(cancellationToken);
        return Map(person);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var person = await _people.GetByIdAsync(id, cancellationToken);
        if (person is null)
            return false;

        if (await _people.HasCreditsAsync(id, cancellationToken))
            throw new ArgumentException("This person is credited on a title and cannot be deleted.");

        _people.Remove(person);
        await _people.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static PersonResponse Map(Person person)
    {
        return new PersonResponse
        {
            Id = person.Id,
            Name = person.Name,
            PhotoUrl = person.PhotoUrl,
            ProfilePath = person.ProfilePath,
            Adult = person.Adult,
            Gender = person.Gender,
            KnownForDepartment = person.KnownForDepartment,
            Popularity = person.Popularity,
            ExternalIds = person.PersonSources
                .OrderBy(link => link.Source?.Name)
                .Select(link => new PersonExternalIdResponse
                {
                    SourceId = link.SourceId,
                    SourceName = link.Source?.Name ?? string.Empty,
                    ExternalId = link.ExternalId
                })
                .ToList(),
            KnownFor = person.KnownFor
                .OrderBy(work => work.SortOrder)
                .Select(work => new PersonKnownForResponse
                {
                    Id = work.Id,
                    SortOrder = work.SortOrder,
                    ExternalId = work.ExternalId,
                    MediaType = work.MediaType,
                    Adult = work.Adult,
                    BackdropPath = work.BackdropPath,
                    PosterPath = work.PosterPath,
                    GenreIds = work.GenreIds.ToList(),
                    OriginalLanguage = work.OriginalLanguage,
                    OriginalTitle = work.OriginalTitle,
                    OriginalName = work.OriginalName,
                    Overview = work.Overview,
                    ReleaseDate = work.ReleaseDate,
                    FirstAirDate = work.FirstAirDate,
                    Title = work.Title,
                    Name = work.Name,
                    Video = work.Video,
                    VoteAverage = work.VoteAverage,
                    VoteCount = work.VoteCount,
                    OriginCountries = work.OriginCountries.ToList()
                })
                .ToList()
        };
    }
}

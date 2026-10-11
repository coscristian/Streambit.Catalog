using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Domain.Aggregates.PersonAggregate;

public class Person
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string PhotoUrl { get; private set; } = string.Empty;
    public string ProfilePath { get; private set; } = string.Empty;
    public bool Adult { get; private set; }
    public int Gender { get; private set; }
    public string KnownForDepartment { get; private set; } = string.Empty;
    public double Popularity { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime LastModified { get; private set; }

    private readonly List<Credit> _credits = [];
    public IReadOnlyCollection<Credit> Credits => _credits;

    private readonly List<PersonSource> _personSources = [];
    public IReadOnlyCollection<PersonSource> PersonSources => _personSources;

    private readonly List<PersonKnownFor> _knownFor = [];
    public IReadOnlyCollection<PersonKnownFor> KnownFor => _knownFor;

    private Person() { }

    public static Person Create(string name, string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(photoUrl))
            throw new ArgumentException("Photo URL is required.", nameof(photoUrl));

        return new Person
        {
            Name = name.Trim(),
            PhotoUrl = photoUrl.Trim(),
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    public static Person Import(
        string name,
        string? photoUrl,
        string? profilePath,
        bool adult,
        int gender,
        string? knownForDepartment,
        double popularity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (gender < 0)
            throw new ArgumentException("Gender cannot be negative.", nameof(gender));

        if (double.IsNaN(popularity) || double.IsInfinity(popularity))
            throw new ArgumentException("Popularity is invalid.", nameof(popularity));

        return new Person
        {
            Name = name.Trim(),
            PhotoUrl = photoUrl?.Trim() ?? string.Empty,
            ProfilePath = profilePath?.Trim() ?? string.Empty,
            Adult = adult,
            Gender = gender,
            KnownForDepartment = knownForDepartment?.Trim() ?? string.Empty,
            Popularity = popularity,
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        var trimmed = name.Trim();
        if (trimmed == Name)
            return;

        Name = trimmed;
        LastModified = DateTime.UtcNow;
    }

    public void UpdatePhoto(string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
            throw new ArgumentException("Photo URL is required.", nameof(photoUrl));

        var trimmed = photoUrl.Trim();
        if (trimmed == PhotoUrl)
            return;

        PhotoUrl = trimmed;
        LastModified = DateTime.UtcNow;
    }

    public void AssignExternalId(int sourceId, string externalId)
    {
        var existing = _personSources.FirstOrDefault(link => link.SourceId == sourceId);
        if (existing is null)
        {
            _personSources.Add(PersonSource.Create(sourceId, externalId));
            return;
        }

        existing.ChangeExternalId(externalId);
    }

    public void AddKnownFor(PersonKnownFor work)
    {
        _knownFor.Add(work);
    }
}

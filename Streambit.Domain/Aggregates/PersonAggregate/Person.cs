using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Domain.Aggregates.PersonAggregate;

public class Person
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string PhotoUrl { get; private set; } = string.Empty;
    public DateTime CreatedDate { get; private set; }
    public DateTime LastModified { get; private set; }

    private readonly List<Credit> _credits = [];
    public IReadOnlyCollection<Credit> Credits => _credits;

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
}

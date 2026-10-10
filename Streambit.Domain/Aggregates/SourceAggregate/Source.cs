namespace Streambit.Domain.Aggregates.SourceAggregate;

public class Source
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateTime CreatedDate { get; private set; }
    public DateTime LastModified { get; private set; }

    private Source() { }

    public static Source Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Source name is required.", nameof(name));

        return new Source
        {
            Name = name.Trim(),
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Source name is required.", nameof(name));

        var trimmed = name.Trim();
        if (trimmed == Name)
            return;

        Name = trimmed;
        LastModified = DateTime.UtcNow;
    }
}

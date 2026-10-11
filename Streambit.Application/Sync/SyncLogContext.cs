namespace Streambit.Application.Sync;

public interface ISyncLogContext
{
    Guid RunId { get; }
    string Entity { get; }
    string Trigger { get; }
    void Begin(string entity, string trigger);
}

public static class SyncEntity
{
    public const string Genre = "Genre";
    public const string Person = "Person";
    public const string Movie = "Movie";
}

public static class SyncTrigger
{
    public const string Manual = "Manual";
    public const string Scheduled = "Scheduled";
}

public class SyncLogContext : ISyncLogContext
{
    public Guid RunId { get; private set; }
    public string Entity { get; private set; } = string.Empty;
    public string Trigger { get; private set; } = string.Empty;

    public void Begin(string entity, string trigger)
    {
        if (string.IsNullOrWhiteSpace(entity))
            throw new ArgumentException("Entity is required.", nameof(entity));

        if (string.IsNullOrWhiteSpace(trigger))
            throw new ArgumentException("Trigger is required.", nameof(trigger));

        RunId = Guid.NewGuid();
        Entity = entity.Trim();
        Trigger = trigger.Trim();
    }
}

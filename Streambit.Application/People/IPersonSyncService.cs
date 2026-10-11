namespace Streambit.Application.People;

public interface IPersonSyncService
{
    Task<PersonSyncResult> SyncAsync(string trigger, CancellationToken cancellationToken = default);
}

public sealed record PersonSyncResult(int Created, int Skipped);

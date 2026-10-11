namespace Streambit.Application.Movies;

public interface IMovieSyncService
{
    Task<MovieSyncResult> SyncAsync(string trigger, CancellationToken cancellationToken = default);
}

public sealed record MovieSyncResult(int Created, int Skipped);

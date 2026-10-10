namespace Streambit.Application.Genres;

public interface IGenreSyncService
{
    Task<GenreSyncResult> SyncAsync(CancellationToken cancellationToken = default);
}

public sealed record GenreSyncResult(int Created, int Linked, int Skipped);

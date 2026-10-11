using Microsoft.Extensions.Options;
using Streambit.Application.Genres;
using Streambit.Application.Movies;
using Streambit.Application.People;
using Streambit.Application.Sync;

namespace Streambit.Api.Tmdb;

public class GenreSyncHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopes;
    private readonly IOptions<GenreSyncOptions> _syncOptions;
    private readonly IOptions<TmdbOptions> _tmdbOptions;
    private readonly ILogger<GenreSyncHostedService> _logger;

    public GenreSyncHostedService(
        IServiceScopeFactory scopes,
        IOptions<GenreSyncOptions> syncOptions,
        IOptions<TmdbOptions> tmdbOptions,
        ILogger<GenreSyncHostedService> logger)
    {
        _scopes = scopes;
        _syncOptions = syncOptions;
        _tmdbOptions = tmdbOptions;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sync = _syncOptions.Value;
        if (!sync.Enabled)
        {
            _logger.LogInformation("TMDB sync is disabled.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_tmdbOptions.Value.ApiKey))
        {
            _logger.LogWarning("TMDB API key is missing. TMDB sync is skipped.");
            return;
        }

        if (sync.Interval <= TimeSpan.Zero)
        {
            _logger.LogWarning("TMDB sync interval must be greater than zero. TMDB sync is skipped.");
            return;
        }

        _logger.LogInformation("TMDB sync scheduled every {Interval}.", sync.Interval);

        using var timer = new PeriodicTimer(sync.Interval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
            await SyncOnceAsync(stoppingToken);
    }

    private async Task SyncOnceAsync(CancellationToken cancellationToken)
    {
        await SyncGenresAsync(cancellationToken);
        await SyncMoviesAsync(cancellationToken);
        await SyncPeopleAsync(cancellationToken);
    }

    private async Task SyncGenresAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = _scopes.CreateAsyncScope();
            var sync = scope.ServiceProvider.GetRequiredService<IGenreSyncService>();
            var result = await sync.SyncAsync(SyncTrigger.Scheduled, cancellationToken);
            _logger.LogInformation(
                "Genre sync finished. Created {Created}, linked {Linked}, skipped {Skipped}.",
                result.Created,
                result.Linked,
                result.Skipped);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Genre sync failed.");
        }
    }

    private async Task SyncMoviesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = _scopes.CreateAsyncScope();
            var sync = scope.ServiceProvider.GetRequiredService<IMovieSyncService>();
            var result = await sync.SyncAsync(SyncTrigger.Scheduled, cancellationToken);
            _logger.LogInformation(
                "Movie sync finished. Created {Created}, skipped {Skipped}.",
                result.Created,
                result.Skipped);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Movie sync failed.");
        }
    }

    private async Task SyncPeopleAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = _scopes.CreateAsyncScope();
            var sync = scope.ServiceProvider.GetRequiredService<IPersonSyncService>();
            var result = await sync.SyncAsync(SyncTrigger.Scheduled, cancellationToken);
            _logger.LogInformation(
                "People sync finished. Created {Created}, skipped {Skipped}.",
                result.Created,
                result.Skipped);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "People sync failed.");
        }
    }
}

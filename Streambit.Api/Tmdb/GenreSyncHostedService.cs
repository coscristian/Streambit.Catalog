using Microsoft.Extensions.Options;
using Streambit.Application.Genres;

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
            _logger.LogInformation("Genre sync is disabled.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_tmdbOptions.Value.ApiKey))
        {
            _logger.LogWarning("TMDB API key is missing. Genre sync is skipped.");
            return;
        }

        if (sync.Interval <= TimeSpan.Zero)
        {
            _logger.LogWarning("Genre sync interval must be greater than zero. Genre sync is skipped.");
            return;
        }

        _logger.LogInformation("Genre sync scheduled every {Interval}.", sync.Interval);

        using var timer = new PeriodicTimer(sync.Interval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
            await SyncOnceAsync(stoppingToken);
    }

    private async Task SyncOnceAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = _scopes.CreateAsyncScope();
            var sync = scope.ServiceProvider.GetRequiredService<IGenreSyncService>();
            var result = await sync.SyncAsync(cancellationToken);
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
}

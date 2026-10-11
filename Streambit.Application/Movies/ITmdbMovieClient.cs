namespace Streambit.Application.Movies;

public interface ITmdbMovieClient
{
    Task<TmdbDiscoverMoviesPage> GetPopularMoviesAsync(int page, CancellationToken cancellationToken = default);
}

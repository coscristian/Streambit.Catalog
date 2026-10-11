namespace Streambit.Application.Genres;

public interface ITmdbGenreClient
{
    Task<IReadOnlyList<TmdbGenre>> GetMovieGenresAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TmdbGenre>> GetTvGenresAsync(CancellationToken cancellationToken = default);
}

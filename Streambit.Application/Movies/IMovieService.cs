using Streambit.Contracts.Movies;

namespace Streambit.Application.Movies;

public interface IMovieService
{
    Task<IReadOnlyList<MovieResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MovieResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MovieResponse> CreateAsync(MovieRequest request, CancellationToken cancellationToken = default);
    Task<MovieResponse?> UpdateAsync(int id, MovieRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Application.Abstractions;

public interface IMovieRepository
{
    Task<Movie?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Movie movie, CancellationToken cancellationToken = default);
    void Remove(Movie movie);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

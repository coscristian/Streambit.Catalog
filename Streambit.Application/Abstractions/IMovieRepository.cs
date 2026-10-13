using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Application.Abstractions;

public interface IMovieRepository
{
    Task<Movie?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Movie> Items, int TotalResults)> GetPageAsync(ContentFormat? format, int? genreId, int page, int pageSize, string orderBy, bool sortAscending, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetExternalIdsAsync(int sourceId, CancellationToken cancellationToken = default);
    Task AddAsync(Movie movie, CancellationToken cancellationToken = default);
    void Remove(Movie movie);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void ClearChangeTracker();
}

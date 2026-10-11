using Streambit.Domain.Aggregates.GenreAggregate;

namespace Streambit.Application.Abstractions;

public interface IGenreRepository
{
    Task<Genre?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> AllExistAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken = default);
    Task<bool> ExternalIdInUseAsync(int sourceId, string externalId, int genreId, CancellationToken cancellationToken = default);
    Task<Genre?> GetByExternalIdAsync(int sourceId, string externalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyDictionary<string, int>> GetIdsByExternalIdAsync(int sourceId, CancellationToken cancellationToken = default);
    Task<Genre?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(Genre genre, CancellationToken cancellationToken = default);
    void Remove(Genre genre);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

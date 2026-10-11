using Streambit.Domain.Aggregates.SourceAggregate;

namespace Streambit.Application.Abstractions;

public interface ISourceRepository
{
    Task<Source?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Source?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Source>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> HasGenresAsync(int sourceId, CancellationToken cancellationToken = default);
    Task AddAsync(Source source, CancellationToken cancellationToken = default);
    void Remove(Source source);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Application.Abstractions;

public interface ICreditRepository
{
    Task<Credit?> GetAsync(int movieId, int personId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Credit>> GetAllAsync(CancellationToken cancellationToken = default);
    void Remove(Credit credit);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

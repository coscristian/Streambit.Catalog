using Streambit.Domain.Aggregates.PersonAggregate;

namespace Streambit.Application.Abstractions;

public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> HasCreditsAsync(int personId, CancellationToken cancellationToken = default);
    Task AddAsync(Person person, CancellationToken cancellationToken = default);
    void Remove(Person person);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

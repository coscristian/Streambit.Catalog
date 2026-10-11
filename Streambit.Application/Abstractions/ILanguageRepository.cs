using Streambit.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Application.Abstractions;

public interface ILanguageRepository
{
    Task<Language?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Language>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Language> languages, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

using Streambit.Contracts.People;

namespace Streambit.Application.People;

public interface IPersonService
{
    Task<IReadOnlyList<PersonResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PersonResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PersonResponse> CreateAsync(PersonRequest request, CancellationToken cancellationToken = default);
    Task<PersonResponse?> UpdateAsync(int id, PersonRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

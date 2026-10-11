using Streambit.Contracts.Credits;

namespace Streambit.Application.Credits;

public interface ICreditService
{
    Task<IReadOnlyList<CreditResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CreditResponse?> GetAsync(int movieId, int personId, CancellationToken cancellationToken = default);
    Task<CreditResponse> CreateAsync(CreditRequest request, CancellationToken cancellationToken = default);
    Task<CreditResponse?> UpdateAsync(int movieId, int personId, CreditRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int movieId, int personId, CancellationToken cancellationToken = default);
}

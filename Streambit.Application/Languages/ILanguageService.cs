using Streambit.Contracts.Dto.Languages.Requests;
using Streambit.Contracts.Languages.Responses;

namespace Streambit.Application.Languages;

public interface ILanguageService
{
    Task<IReadOnlyList<LanguageResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LanguageResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LanguageResponse>> CreateAsync(IReadOnlyList<LanguageCreate> requests, CancellationToken cancellationToken = default);
}

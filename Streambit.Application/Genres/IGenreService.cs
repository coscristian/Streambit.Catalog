using Streambit.Contracts.Genres;

namespace Streambit.Application.Genres;

public interface IGenreService
{
    Task<IReadOnlyList<GenreResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GenreResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<GenreResponse> CreateAsync(GenreRequest request, CancellationToken cancellationToken = default);
    Task<GenreResponse?> UpdateAsync(int id, GenreRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<GenreResponse?> AssignExternalIdAsync(int id, GenreExternalIdRequest request, CancellationToken cancellationToken = default);
    Task<GenreResponse?> RemoveExternalIdAsync(int id, int sourceId, CancellationToken cancellationToken = default);
}

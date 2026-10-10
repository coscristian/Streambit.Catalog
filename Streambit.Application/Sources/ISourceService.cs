using Streambit.Application.Abstractions;
using Streambit.Contracts.Sources;
using Streambit.Domain.Aggregates.SourceAggregate;

namespace Streambit.Application.Sources;

public interface ISourceService
{
    Task<IReadOnlyList<SourceResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SourceResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SourceResponse> CreateAsync(SourceRequest request, CancellationToken cancellationToken = default);
    Task<SourceResponse?> UpdateAsync(int id, SourceRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

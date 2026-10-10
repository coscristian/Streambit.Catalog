using Streambit.Application.Abstractions;
using Streambit.Contracts.Sources;
using Streambit.Domain.Aggregates.SourceAggregate;

namespace Streambit.Application.Sources;

public class SourceService : ISourceService
{
    private readonly ISourceRepository _sources;

    public SourceService(ISourceRepository sources)
    {
        _sources = sources;
    }

    public async Task<IReadOnlyList<SourceResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sources = await _sources.GetAllAsync(cancellationToken);
        return sources.Select(Map).ToList();
    }

    public async Task<SourceResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var source = await _sources.GetByIdAsync(id, cancellationToken);
        return source is null ? null : Map(source);
    }

    public async Task<SourceResponse> CreateAsync(SourceRequest request, CancellationToken cancellationToken = default)
    {
        var source = Source.Create(request.Name);
        await _sources.AddAsync(source, cancellationToken);
        await _sources.SaveChangesAsync(cancellationToken);
        return Map(source);
    }

    public async Task<SourceResponse?> UpdateAsync(int id, SourceRequest request, CancellationToken cancellationToken = default)
    {
        var source = await _sources.GetByIdAsync(id, cancellationToken);
        if (source is null)
            return null;

        source.UpdateName(request.Name);
        await _sources.SaveChangesAsync(cancellationToken);
        return Map(source);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var source = await _sources.GetByIdAsync(id, cancellationToken);
        if (source is null)
            return false;

        if (await _sources.HasGenresAsync(id, cancellationToken))
            throw new ArgumentException("This source is linked to genres and cannot be deleted.");

        _sources.Remove(source);
        await _sources.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static SourceResponse Map(Source source)
    {
        return new SourceResponse
        {
            Id = source.Id,
            Name = source.Name
        };
    }
}

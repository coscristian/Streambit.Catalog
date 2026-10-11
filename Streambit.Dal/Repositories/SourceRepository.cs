using Microsoft.EntityFrameworkCore;
using Streambit.Application.Abstractions;
using Streambit.Domain.Aggregates.SourceAggregate;

namespace Streambit.Dal.Repositories;

public class SourceRepository : ISourceRepository
{
    private readonly DataContext _context;

    public SourceRepository(DataContext context)
    {
        _context = context;
    }

    public Task<Source?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Sources.FirstOrDefaultAsync(source => source.Id == id, cancellationToken);
    }

    public Task<Source?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalized = name.Trim().ToLower();
        return _context.Sources.FirstOrDefaultAsync(source => source.Name.ToLower() == normalized, cancellationToken);
    }

    public async Task<IReadOnlyList<Source>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Sources
            .OrderBy(source => source.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> HasGenresAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        return _context.GenreSources.AnyAsync(link => link.SourceId == sourceId, cancellationToken);
    }

    public Task AddAsync(Source source, CancellationToken cancellationToken = default)
    {
        return _context.Sources.AddAsync(source, cancellationToken).AsTask();
    }

    public void Remove(Source source)
    {
        _context.Sources.Remove(source);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}

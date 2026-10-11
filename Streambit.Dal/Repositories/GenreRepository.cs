using Microsoft.EntityFrameworkCore;
using Streambit.Application.Abstractions;
using Streambit.Domain.Aggregates.GenreAggregate;

namespace Streambit.Dal.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly DataContext _context;

    public GenreRepository(DataContext context)
    {
        _context = context;
    }

    public Task<Genre?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Query().FirstOrDefaultAsync(genre => genre.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .OrderBy(genre => genre.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Genre?> GetByExternalIdAsync(int sourceId, string externalId, CancellationToken cancellationToken = default)
    {
        return Query().FirstOrDefaultAsync(
            genre => genre.GenreSources.Any(link => link.SourceId == sourceId && link.ExternalId == externalId),
            cancellationToken);
    }

    public async Task<IReadOnlyDictionary<string, int>> GetIdsByExternalIdAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        var links = await _context.GenreSources
            .Where(link => link.SourceId == sourceId)
            .Select(link => new { link.ExternalId, link.GenreId })
            .ToListAsync(cancellationToken);

        return links.ToDictionary(link => link.ExternalId, link => link.GenreId, StringComparer.Ordinal);
    }

    public Task<Genre?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalized = name.Trim().ToLower();
        return Query().FirstOrDefaultAsync(genre => genre.Name.ToLower() == normalized, cancellationToken);
    }

    public Task<bool> ExternalIdInUseAsync(int sourceId, string externalId, int genreId, CancellationToken cancellationToken = default)
    {
        return _context.GenreSources.AnyAsync(
            link => link.SourceId == sourceId
                && link.ExternalId == externalId
                && link.GenreId != genreId,
            cancellationToken);
    }

    public async Task<bool> AllExistAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken = default)
    {
        if (ids.Count == 0)
            return true;

        var found = await _context.Genres.CountAsync(genre => ids.Contains(genre.Id), cancellationToken);
        return found == ids.Count;
    }

    public Task AddAsync(Genre genre, CancellationToken cancellationToken = default)
    {
        return _context.Genres.AddAsync(genre, cancellationToken).AsTask();
    }

    public void Remove(Genre genre)
    {
        _context.Genres.Remove(genre);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Genre> Query()
    {
        return _context.Genres
            .Include(genre => genre.GenreSources)
                .ThenInclude(link => link.Source);
    }
}

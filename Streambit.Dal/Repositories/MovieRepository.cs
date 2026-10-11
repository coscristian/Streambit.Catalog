using Microsoft.EntityFrameworkCore;
using Streambit.Application.Abstractions;
using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Dal.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly DataContext _context;

    public MovieRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Movie?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (movie is null)
            return null;

        await LoadDetailsAsync(movie, cancellationToken);
        return movie;
    }

    public async Task<(IReadOnlyList<Movie> Items, int TotalResults)> GetPageAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var total = await _context.Movies.CountAsync(cancellationToken);
        var items = await _context.Movies
            .AsNoTracking()
            .AsSplitQuery()
            .Include(movie => movie.MovieGenres)
                .ThenInclude(link => link.Genre)
            .Include(movie => movie.Credits)
                .ThenInclude(credit => credit.Person)
            .Include(movie => movie.MovieSources)
                .ThenInclude(link => link.Source)
            .OrderBy(movie => movie.Title)
            .ThenBy(movie => movie.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<IReadOnlyList<string>> GetExternalIdsAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        return await _context.MovieSources
            .Where(link => link.SourceId == sourceId)
            .Select(link => link.ExternalId)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        return _context.Movies.AddAsync(movie, cancellationToken).AsTask();
    }

    public void Remove(Movie movie)
    {
        _context.Movies.Remove(movie);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void ClearChangeTracker()
    {
        _context.ChangeTracker.Clear();
    }

    private async Task LoadDetailsAsync(Movie movie, CancellationToken cancellationToken)
    {
        await _context.Entry(movie)
            .Collection(item => item.MovieGenres)
            .Query()
            .Include(link => link.Genre)
            .LoadAsync(cancellationToken);

        await _context.Entry(movie)
            .Collection(item => item.Credits)
            .Query()
            .Include(credit => credit.Person)
            .LoadAsync(cancellationToken);

        await _context.Entry(movie)
            .Collection(item => item.MovieSources)
            .Query()
            .Include(link => link.Source)
            .LoadAsync(cancellationToken);
    }
}

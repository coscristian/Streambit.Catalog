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
        ContentFormat? format,
        int? genreId,
        int page,
        int pageSize,
        string orderBy,
        bool sortAscending,
        CancellationToken cancellationToken = default)
    {
        var filtered = _context.Movies.AsQueryable();
        if (format is not null && format != ContentFormat.All)
            filtered = filtered.Where(movie => movie.Format == format);

        if (genreId is > 0)
            filtered = filtered.Where(movie => movie.MovieGenres.Any(link => link.GenreId == genreId));

        filtered = ApplyOrderBy(filtered, orderBy, sortAscending);

        var total = await filtered.CountAsync(cancellationToken);
        var items = await filtered
            .AsNoTracking()
            .Include(movie => movie.MovieGenres)
                .ThenInclude(link => link.Genre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    private IQueryable<Movie> ApplyOrderBy(IQueryable<Movie> query, string orderBy, bool sortAscending)
    {
        switch (orderBy.ToLowerInvariant())
        {
            case "id":
                return sortAscending ? query.OrderBy(movie => movie.Id) : query.OrderByDescending(movie => movie.Id);
            case "title":
                return sortAscending ? query.OrderBy(movie => movie.Title) : query.OrderByDescending(movie => movie.Title);
            case "releaseDate":
                return sortAscending ? query.OrderBy(movie => movie.ReleaseDate) : query.OrderByDescending(movie => movie.ReleaseDate);
            case "popularity":
                return sortAscending ? query.OrderBy(movie => movie.Popularity) : query.OrderByDescending(movie => movie.Popularity);
            default:
                return query.OrderBy(movie => movie.Id);
        }
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

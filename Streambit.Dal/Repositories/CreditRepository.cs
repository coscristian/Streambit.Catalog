using Microsoft.EntityFrameworkCore;
using Streambit.Application.Abstractions;
using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Dal.Repositories;

public class CreditRepository : ICreditRepository
{
    private readonly DataContext _context;

    public CreditRepository(DataContext context)
    {
        _context = context;
    }

    public Task<Credit?> GetAsync(int movieId, int personId, CancellationToken cancellationToken = default)
    {
        return Query().FirstOrDefaultAsync(
            credit => credit.MovieId == movieId && credit.PersonId == personId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Credit>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .OrderBy(credit => credit.Movie.Title)
            .ThenBy(credit => credit.BillingOrder)
            .ToListAsync(cancellationToken);
    }

    public void Remove(Credit credit)
    {
        _context.Credits.Remove(credit);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Credit> Query()
    {
        return _context.Credits
            .Include(credit => credit.Movie)
            .Include(credit => credit.Person);
    }
}

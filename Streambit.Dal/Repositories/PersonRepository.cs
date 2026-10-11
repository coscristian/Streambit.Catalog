using Microsoft.EntityFrameworkCore;
using Streambit.Application.Abstractions;
using Streambit.Domain.Aggregates.PersonAggregate;

namespace Streambit.Dal.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly DataContext _context;

    public PersonRepository(DataContext context)
    {
        _context = context;
    }

    public Task<Person?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Query().FirstOrDefaultAsync(person => person.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .OrderBy(person => person.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> HasCreditsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return _context.Credits.AnyAsync(credit => credit.PersonId == personId, cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetExternalIdsAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        return await _context.PersonSources
            .Where(link => link.SourceId == sourceId)
            .Select(link => link.ExternalId)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(Person person, CancellationToken cancellationToken = default)
    {
        return _context.People.AddAsync(person, cancellationToken).AsTask();
    }

    public void Remove(Person person)
    {
        _context.People.Remove(person);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void ClearChangeTracker()
    {
        _context.ChangeTracker.Clear();
    }

    private IQueryable<Person> Query()
    {
        return _context.People
            .Include(person => person.KnownFor)
            .Include(person => person.PersonSources)
                .ThenInclude(link => link.Source);
    }
}

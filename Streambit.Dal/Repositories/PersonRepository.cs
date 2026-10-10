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
        return _context.People.FirstOrDefaultAsync(person => person.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.People
            .OrderBy(person => person.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> HasCreditsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return _context.Credits.AnyAsync(credit => credit.PersonId == personId, cancellationToken);
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
}

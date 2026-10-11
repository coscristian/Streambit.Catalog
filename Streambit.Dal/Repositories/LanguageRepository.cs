using Microsoft.EntityFrameworkCore;
using Streambit.Application.Abstractions;
using Streambit.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Dal.Repositories;

public class LanguageRepository : ILanguageRepository
{
    private readonly DataContext _context;

    public LanguageRepository(DataContext context)
    {
        _context = context;
    }

    public Task<Language?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Languages.FirstOrDefaultAsync(language => language.LanguageId == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Language>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Languages
            .OrderBy(language => language.Name)
            .ToListAsync(cancellationToken);
    }

    public Task AddRangeAsync(IEnumerable<Language> languages, CancellationToken cancellationToken = default)
    {
        return _context.Languages.AddRangeAsync(languages, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}

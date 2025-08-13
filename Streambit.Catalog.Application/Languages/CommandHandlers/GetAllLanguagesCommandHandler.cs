using MediatR;
using Microsoft.EntityFrameworkCore;
using Streambit.Catalog.Application.Languages.Queries;
using Streambit.Catalog.Dal;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Application.Languages.CommandHandlers;

public class GetAllLanguagesCommandHandler :  IRequestHandler<GetAllLanguagesQuery, IEnumerable<Language>>
{
    private readonly DataContext _context;
    
    public GetAllLanguagesCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Language>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Languages.ToListAsync();
    }
}
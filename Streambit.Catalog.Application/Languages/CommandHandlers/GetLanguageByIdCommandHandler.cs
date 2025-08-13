using MediatR;
using Microsoft.EntityFrameworkCore;
using Streambit.Catalog.Application.Languages.Queries;
using Streambit.Catalog.Dal;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Application.Languages.CommandHandlers;

public class GetLanguageByIdCommandHandler : IRequestHandler<GetLanguageById, Language?>
{
    private readonly DataContext _context;
    
    public GetLanguageByIdCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<Language?> Handle(GetLanguageById request, CancellationToken cancellationToken)
    {
        return await _context.Languages.FirstOrDefaultAsync(l => l.LanguageId == request.LanguageId); 
    }
}
using MediatR;
using Streambit.Catalog.Application.Languages.Commands;
using Streambit.Catalog.Dal;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Application.Languages.CommandHandlers;

public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, Language>
{
    private readonly DataContext _context;
        
    public CreateLanguageCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<Language> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        var language = Language.CreateLanguage(request.Name, request.EnglishName, request.Iso6391);
        
        _context.Languages.Add(language);
        await _context.SaveChangesAsync(cancellationToken);
        
        return language;
    }
}
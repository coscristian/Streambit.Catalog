using MediatR;
using Streambit.Catalog.Application.Languages.Commands;
using Streambit.Catalog.Dal;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Application.Languages.CommandHandlers;

public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguagesCommand, List<Language>>
{
    private readonly DataContext _context;
        
    public CreateLanguageCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<List<Language>> Handle(CreateLanguagesCommand request, CancellationToken cancellationToken)
    {
        // TODO: Avoid duplications
        var newLanguages = request.Languages
            .Select(newLang => Language.CreateLanguage(newLang.Name, newLang.EnglishName, newLang.Iso6391))
            .ToList();
        
        await _context.Languages.AddRangeAsync(newLanguages, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return newLanguages;
    }
}
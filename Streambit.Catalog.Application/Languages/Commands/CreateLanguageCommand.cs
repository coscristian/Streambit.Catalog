using MediatR;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Application.Languages.Commands;

public class CreateLanguageCommand : IRequest<Language>
{
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public string Iso6391 { get; set; }
}
using MediatR;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Application.Languages.Queries;

public class GetAllLanguagesQuery : IRequest<IEnumerable<Language>>
{
    
}
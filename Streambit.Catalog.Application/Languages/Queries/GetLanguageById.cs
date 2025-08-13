using MediatR;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Application.Languages.Queries;

public class GetLanguageById : IRequest<Language>
{
    public Guid LanguageId { get; set; }
}
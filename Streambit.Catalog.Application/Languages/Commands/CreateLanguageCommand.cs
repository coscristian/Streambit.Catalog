using MediatR;
using Streambit.Catalog.Contracts.Dto.Languages.Requests;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;
namespace Streambit.Catalog.Application.Languages.Commands;

public class CreateLanguagesCommand : IRequest<List<Language>>
{
    public List<LanguageCreateDto> Languages { get; set; } = [];
}
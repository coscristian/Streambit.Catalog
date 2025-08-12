using AutoMapper;
using Streambit.Catalog.Application.Languages.Commands;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Application.MappingProfiles;

public class LanguageMap : Profile
{
    public LanguageMap()
    {
        CreateMap<CreateLanguageCommand, Language>();
    }
}
using AutoMapper;
using Streambit.Catalog.Application.Languages.Commands;
using Streambit.Catalog.Application.Languages.Queries;
using Streambit.Catalog.Contracts.Dto.Languages.Requests;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Application.MappingProfiles;

public class LanguageMap : Profile
{
    public LanguageMap()
    {
        CreateMap<List<LanguageCreateDto>, CreateLanguagesCommand>()
            .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src));
    }
}
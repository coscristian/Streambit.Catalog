using AutoMapper;
using Streambit.Catalog.Application.Genres.Commands;
using Streambit.Catalog.Contracts.Dto.Genres.Requests;
using Streambit.Catalog.Contracts.Dto.Genres.Responses;
using Streambit.Catalog.Domain.Aggregates.GenreAggregate;

namespace Streambit.Catalog.Application.MappingProfiles;

public class GenreMap : Profile
{
    public GenreMap()
    {
        CreateMap<List<GenreCreateRequestDto>, CreateGenreCommand>()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src));

        CreateMap<Genre, GenreCreateResponseDto>();
    }
}
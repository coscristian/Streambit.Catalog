using MediatR;
using Streambit.Catalog.Contracts.Dto.Genres.Requests;
using Streambit.Catalog.Contracts.Dto.Genres.Responses;
using Streambit.Catalog.Domain.Aggregates.GenreAggregate;

namespace Streambit.Catalog.Application.Genres.Commands;

public class CreateGenreCommand : IRequest<List<GenreCreateResponseDto>>
{
    public List<GenreCreateRequestDto> Genres { get; } = [];
    
}
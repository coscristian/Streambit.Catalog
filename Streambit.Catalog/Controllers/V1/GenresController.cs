using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streambit.Catalog.Application.Genres.Commands;
using Streambit.Catalog.Contracts.Dto.Genres.Requests;

namespace Streambit.Catalog.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class GenresController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GenresController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // TODO: Implementar endpoint de obtención de generos de peliculas
        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            return (IActionResult)Task.FromResult(Ok());
        }
        
        [HttpGet]
        [Route(ApiRoutes.Genres.GetById)]
        public async Task<IActionResult> GetById(int id)
        {
            return (IActionResult)Task.FromResult(Ok());
        }
        
        [HttpPost]
        [Route(ApiRoutes.Genres.Create)]
        public async Task<IActionResult> CreateGenre([FromBody] List<GenreCreateRequestDto> genre)
        {
            var command = _mapper.Map<CreateGenreCommand>(genre);
            var response = await _mediator.Send(command);
            return Created("", response );
        }
    }
}

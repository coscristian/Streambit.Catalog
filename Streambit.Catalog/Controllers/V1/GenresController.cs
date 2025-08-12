using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Streambit.Catalog.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class GenresController : Controller
    {
        private readonly IMediator _mediator;

        public GenresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            return (IActionResult)Task.FromResult(Ok());
        }
        
        [HttpGet]
        [Route(ApiRoutes.Movies.GetById)]
        public async Task<IActionResult> GetById(int id)
        {
            return (IActionResult)Task.FromResult(Ok());
        }
    }
}

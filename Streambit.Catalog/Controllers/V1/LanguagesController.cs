using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streambit.Catalog.Application.Languages.Commands;
using Streambit.Catalog.Contracts.Languages.Requests;

namespace Streambit.Catalog.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class LanguagesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LanguagesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllLanguages()
        {
            return (IActionResult)Task.FromResult(Ok());
        }
        
        [HttpGet]
        [Route(ApiRoutes.Languages.GetById)]
        public IActionResult GetLanguageById()
        {
            return (IActionResult)Task.FromResult(Ok());
        }

        [HttpPost]
        [Route(ApiRoutes.Languages.CreateLanguage)]
        public async Task<IActionResult> CreateLanguage([FromBody] LanguageCreate newLanguage)
        {
            var command = _mapper.Map<CreateLanguageCommand>(newLanguage);
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetLanguageById), new { id = response.LanguageId, response });
        }
    }
}

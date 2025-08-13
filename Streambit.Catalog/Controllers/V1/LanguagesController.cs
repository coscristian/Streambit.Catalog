using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streambit.Catalog.Application.Languages.Commands;
using Streambit.Catalog.Application.Languages.Queries;
using Streambit.Catalog.Contracts.Languages.Requests;
using Streambit.Catalog.Contracts.Languages.Responses;

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
        public async Task<IActionResult> GetAllLanguages()
        {
            var query = new GetAllLanguagesQuery();
            var response =  await _mediator.Send(query);
            var languages = _mapper.Map<List<LanguageResponse>>(response);
            return Ok(languages);
        }
        
        [HttpGet]
        [Route(ApiRoutes.Languages.GetById)]
        public async Task<IActionResult> GetLanguageById(string id)
        {
            var query = new GetLanguageById() { LanguageId = Guid.Parse(id) };
            
            var response =  await _mediator.Send(query);
            var language = _mapper.Map<LanguageResponse>(response);
            return Ok(language);
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

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Streambit.Application.Languages;
using Streambit.Contracts.Dto.Languages.Requests;
using Streambit.Contracts.Languages.Responses;

namespace Streambit.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class LanguagesController : ControllerBase
{
    private readonly ILanguageService _languages;

    public LanguagesController(ILanguageService languages)
    {
        _languages = languages;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LanguageResponse>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _languages.GetAllAsync(cancellationToken));
    }

    [HttpGet(ApiRoutes.Languages.GetById)]
    public async Task<ActionResult<LanguageResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var language = await _languages.GetByIdAsync(id, cancellationToken);
        return language is null ? NotFound() : Ok(language);
    }

    [HttpPost(ApiRoutes.Languages.CreateLanguages)]
    public async Task<ActionResult<IReadOnlyList<LanguageResponse>>> Create(
        [FromBody] List<LanguageCreate> newLanguages,
        CancellationToken cancellationToken)
    {
        var languages = await _languages.CreateAsync(newLanguages, cancellationToken);
        return Created(string.Empty, languages);
    }
}

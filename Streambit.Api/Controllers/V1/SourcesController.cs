using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Streambit.Application.Sources;
using Streambit.Contracts.Sources;

namespace Streambit.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class SourcesController : ControllerBase
{
    private readonly ISourceService _sources;

    public SourcesController(ISourceService sources)
    {
        _sources = sources;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SourceResponse>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _sources.GetAllAsync(cancellationToken));
    }

    [HttpGet(ApiRoutes.Sources.GetById)]
    public async Task<ActionResult<SourceResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var source = await _sources.GetByIdAsync(id, cancellationToken);
        return source is null ? NotFound() : Ok(source);
    }

    [HttpPost]
    public async Task<ActionResult<SourceResponse>> Create(SourceRequest request, CancellationToken cancellationToken)
    {
        var source = await _sources.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = source.Id }, source);
    }

    [HttpPut(ApiRoutes.Sources.GetById)]
    public async Task<ActionResult<SourceResponse>> Update(int id, SourceRequest request, CancellationToken cancellationToken)
    {
        var source = await _sources.UpdateAsync(id, request, cancellationToken);
        return source is null ? NotFound() : Ok(source);
    }

    [HttpDelete(ApiRoutes.Sources.GetById)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _sources.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

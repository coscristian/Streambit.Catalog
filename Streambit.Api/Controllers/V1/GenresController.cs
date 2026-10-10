using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Streambit.Application.Genres;
using Streambit.Contracts.Genres;

namespace Streambit.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genres;
    private readonly IGenreSyncService _sync;

    public GenresController(IGenreService genres, IGenreSyncService sync)
    {
        _genres = genres;
        _sync = sync;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GenreResponse>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _genres.GetAllAsync(cancellationToken));
    }

    [HttpGet(ApiRoutes.Genres.GetById)]
    public async Task<ActionResult<GenreResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var genre = await _genres.GetByIdAsync(id, cancellationToken);
        return genre is null ? NotFound() : Ok(genre);
    }

    [HttpPost(ApiRoutes.Genres.Sync)]
    public async Task<ActionResult<GenreSyncResult>> Sync(CancellationToken cancellationToken)
    {
        return Ok(await _sync.SyncAsync(cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<GenreResponse>> Create(GenreRequest request, CancellationToken cancellationToken)
    {
        var genre = await _genres.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = genre.Id }, genre);
    }

    [HttpPut(ApiRoutes.Genres.GetById)]
    public async Task<ActionResult<GenreResponse>> Update(int id, GenreRequest request, CancellationToken cancellationToken)
    {
        var genre = await _genres.UpdateAsync(id, request, cancellationToken);
        return genre is null ? NotFound() : Ok(genre);
    }

    [HttpDelete(ApiRoutes.Genres.GetById)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _genres.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPut(ApiRoutes.Genres.ExternalId)]
    public async Task<ActionResult<GenreResponse>> AssignExternalId(
        int id,
        GenreExternalIdRequest request,
        CancellationToken cancellationToken)
    {
        var genre = await _genres.AssignExternalIdAsync(id, request, cancellationToken);
        return genre is null ? NotFound() : Ok(genre);
    }

    [HttpDelete(ApiRoutes.Genres.ExternalIdBySource)]
    public async Task<ActionResult<GenreResponse>> RemoveExternalId(
        int id,
        int sourceId,
        CancellationToken cancellationToken)
    {
        var genre = await _genres.RemoveExternalIdAsync(id, sourceId, cancellationToken);
        return genre is null ? NotFound() : Ok(genre);
    }
}

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Streambit.Application.Movies;
using Streambit.Application.Sync;
using Streambit.Contracts.Movies;
using Streambit.Contracts.Titles;

namespace Streambit.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class TitlesController : ControllerBase
{
    private readonly IMovieService _movies;
    private readonly IMovieSyncService _sync;

    public TitlesController(IMovieService movies, IMovieSyncService sync)
    {
        _movies = movies;
        _sync = sync;
    }

    [HttpGet]
    public async Task<ActionResult<TitlePageResponse>> GetAll(
        [FromQuery] TitleListRequest request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _movies.GetPageAsync(request, cancellationToken));
    }

    [HttpGet(ApiRoutes.Titles.GetById)]
    public async Task<ActionResult<MovieResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var movie = await _movies.GetByIdAsync(id, cancellationToken);
        return movie is null ? NotFound() : Ok(movie);
    }

    [HttpPost(ApiRoutes.Titles.Sync)]
    public async Task<ActionResult<MovieSyncResult>> Sync(CancellationToken cancellationToken)
    {
        return Ok(await _sync.SyncAsync(SyncTrigger.Manual, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<MovieResponse>> Create(MovieRequest request, CancellationToken cancellationToken)
    {
        var movie = await _movies.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
    }

    [HttpPut(ApiRoutes.Titles.GetById)]
    public async Task<ActionResult<MovieResponse>> Update(int id, MovieRequest request, CancellationToken cancellationToken)
    {
        var movie = await _movies.UpdateAsync(id, request, cancellationToken);
        return movie is null ? NotFound() : Ok(movie);
    }

    [HttpDelete(ApiRoutes.Titles.GetById)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _movies.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

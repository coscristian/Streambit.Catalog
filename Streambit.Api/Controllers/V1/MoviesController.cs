using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Streambit.Application.Movies;
using Streambit.Application.Sync;
using Streambit.Contracts.Movies;

namespace Streambit.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movies;
    private readonly IMovieSyncService _sync;

    public MoviesController(IMovieService movies, IMovieSyncService sync)
    {
        _movies = movies;
        _sync = sync;
    }

    [HttpGet]
    public async Task<ActionResult<MoviePageResponse>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = MovieService.DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _movies.GetPageAsync(page, pageSize, cancellationToken));
    }

    [HttpGet(ApiRoutes.Movies.GetById)]
    public async Task<ActionResult<MovieResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var movie = await _movies.GetByIdAsync(id, cancellationToken);
        return movie is null ? NotFound() : Ok(movie);
    }

    [HttpPost(ApiRoutes.Movies.Sync)]
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

    [HttpPut(ApiRoutes.Movies.GetById)]
    public async Task<ActionResult<MovieResponse>> Update(int id, MovieRequest request, CancellationToken cancellationToken)
    {
        var movie = await _movies.UpdateAsync(id, request, cancellationToken);
        return movie is null ? NotFound() : Ok(movie);
    }

    [HttpDelete(ApiRoutes.Movies.GetById)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _movies.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Streambit.Application.Credits;
using Streambit.Contracts.Credits;

namespace Streambit.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class CreditsController : ControllerBase
{
    private readonly ICreditService _credits;

    public CreditsController(ICreditService credits)
    {
        _credits = credits;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CreditResponse>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _credits.GetAllAsync(cancellationToken));
    }

    [HttpGet(ApiRoutes.Credits.GetById)]
    public async Task<ActionResult<CreditResponse>> GetById(int movieId, int personId, CancellationToken cancellationToken)
    {
        var credit = await _credits.GetAsync(movieId, personId, cancellationToken);
        return credit is null ? NotFound() : Ok(credit);
    }

    [HttpPost]
    public async Task<ActionResult<CreditResponse>> Create(CreditRequest request, CancellationToken cancellationToken)
    {
        var credit = await _credits.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { movieId = credit.MovieId, personId = credit.PersonId },
            credit);
    }

    [HttpPut(ApiRoutes.Credits.GetById)]
    public async Task<ActionResult<CreditResponse>> Update(
        int movieId,
        int personId,
        CreditRequest request,
        CancellationToken cancellationToken)
    {
        var credit = await _credits.UpdateAsync(movieId, personId, request, cancellationToken);
        return credit is null ? NotFound() : Ok(credit);
    }

    [HttpDelete(ApiRoutes.Credits.GetById)]
    public async Task<IActionResult> Delete(int movieId, int personId, CancellationToken cancellationToken)
    {
        var deleted = await _credits.DeleteAsync(movieId, personId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

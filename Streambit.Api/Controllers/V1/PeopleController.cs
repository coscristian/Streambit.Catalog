using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Streambit.Application.People;
using Streambit.Contracts.People;

namespace Streambit.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly IPersonService _people;

    public PeopleController(IPersonService people)
    {
        _people = people;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PersonResponse>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _people.GetAllAsync(cancellationToken));
    }

    [HttpGet(ApiRoutes.People.GetById)]
    public async Task<ActionResult<PersonResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var person = await _people.GetByIdAsync(id, cancellationToken);
        return person is null ? NotFound() : Ok(person);
    }

    [HttpPost]
    public async Task<ActionResult<PersonResponse>> Create(PersonRequest request, CancellationToken cancellationToken)
    {
        var person = await _people.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
    }

    [HttpPut(ApiRoutes.People.GetById)]
    public async Task<ActionResult<PersonResponse>> Update(int id, PersonRequest request, CancellationToken cancellationToken)
    {
        var person = await _people.UpdateAsync(id, request, cancellationToken);
        return person is null ? NotFound() : Ok(person);
    }

    [HttpDelete(ApiRoutes.People.GetById)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _people.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Streambit.Catalog.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var movie = new { Id = id, Title = "Pirates of the caribbean V2" };
            return Ok(movie);
        }
    }
}

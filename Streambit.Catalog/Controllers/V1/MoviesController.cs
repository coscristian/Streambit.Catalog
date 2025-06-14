using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Streambit.Catalog.Domain.Models;

namespace Streambit.Catalog.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var movie = new Movie { Id = id, Title = "Pirates of the Caribean" };
            return Ok(movie);
        }
    }
}

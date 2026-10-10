using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Streambit.Api.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is KeyNotFoundException notFound)
        {
            context.Result = new NotFoundObjectResult(new { error = notFound.Message });
            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is ArgumentException argument)
        {
            context.Result = new BadRequestObjectResult(new { error = argument.Message });
            context.ExceptionHandled = true;
        }
    }
}

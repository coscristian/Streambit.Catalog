using Streambit.Application;

namespace Streambit.Api.Registrars;

public class ApplicationRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddApplication();
    }
}

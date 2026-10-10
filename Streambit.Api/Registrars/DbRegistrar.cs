using Streambit.Dal;

namespace Streambit.Api.Registrars;

public class DbRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddPersistence(builder.Configuration);
    }
}

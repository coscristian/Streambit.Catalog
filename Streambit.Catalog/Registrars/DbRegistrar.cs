using Microsoft.EntityFrameworkCore;
using Streambit.Catalog.Dal;

namespace Streambit.Catalog.Api.Registrars;

public class DbRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
    }
}
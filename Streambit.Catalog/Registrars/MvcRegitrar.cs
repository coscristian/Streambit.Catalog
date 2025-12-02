
using System.Reflection;
using Asp.Versioning;
using Streambit.Catalog.Application;
using Streambit.Catalog.Application.Languages.CommandHandlers;

namespace Streambit.Catalog.Api.Registrars
{
    public class MvcRegitrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
                config.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(cfg => { }, typeof(AssemblyMarker));
            builder.Services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
        }
    }
}

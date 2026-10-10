using Asp.Versioning;
using Streambit.Api.Filters;

namespace Streambit.Api.Registrars;

public class MvcRegitrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ApiExceptionFilter>();
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
        });
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
    }
}

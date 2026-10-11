using Asp.Versioning;
using Streambit.Api.Filters;

namespace Streambit.Api.Registrars;

public class MvcRegitrar : IWebApplicationBuilderRegistrar
{
    public const string WebPolicy = "Web";

    public void RegisterServices(WebApplicationBuilder builder)
    {
        var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(WebPolicy, policy =>
            {
                if (origins.Length == 0)
                    return;

                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

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

using Streambit.Api.Tmdb;
using Streambit.Application.Genres;

namespace Streambit.Api.Registrars;

public class GenreSyncRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<TmdbOptions>(builder.Configuration.GetSection(TmdbOptions.SectionName));
        builder.Services.Configure<GenreSyncOptions>(builder.Configuration.GetSection(GenreSyncOptions.SectionName));

        builder.Services.AddHttpClient<ITmdbGenreClient, TmdbGenreClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<TmdbOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
        });

        builder.Services.AddScoped<IGenreSyncService, GenreSyncService>();
        builder.Services.AddHostedService<GenreSyncHostedService>();
    }
}

using Microsoft.Extensions.DependencyInjection;
using Streambit.Application.Credits;
using Streambit.Application.Genres;
using Streambit.Application.Languages;
using Streambit.Application.Movies;
using Streambit.Application.People;
using Streambit.Application.Sources;

namespace Streambit.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<ISourceService, SourceService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<ICreditService, CreditService>();
        services.AddScoped<ILanguageService, LanguageService>();
        return services;
    }
}

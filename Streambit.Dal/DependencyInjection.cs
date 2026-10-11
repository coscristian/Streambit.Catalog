using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Streambit.Application.Abstractions;
using Streambit.Dal.Repositories;

namespace Streambit.Dal;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<ISourceRepository, SourceRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<ICreditRepository, CreditRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<ISyncLogRepository, SyncLogRepository>();

        return services;
    }
}

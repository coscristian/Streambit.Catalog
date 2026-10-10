using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Streambit.Dal.Configurations;
using Streambit.Domain.Aggregates.GenreAggregate;
using Streambit.Domain.Aggregates.LanguageAggregate;
using Streambit.Domain.Aggregates.MovieAggregate;
using Streambit.Domain.Aggregates.PersonAggregate;
using Streambit.Domain.Aggregates.SourceAggregate;

namespace Streambit.Dal;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Credit> Credits { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<GenreSource> GenreSources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MovieConfig());
        modelBuilder.ApplyConfiguration(new GenreConfig());
        modelBuilder.ApplyConfiguration(new MovieGenreConfig());
        modelBuilder.ApplyConfiguration(new LanguageConfig());
        modelBuilder.ApplyConfiguration(new PersonConfig());
        modelBuilder.ApplyConfiguration(new CreditConfig());
        modelBuilder.ApplyConfiguration(new SourceConfig());
        modelBuilder.ApplyConfiguration(new GenreSourceConfig());

        modelBuilder.ApplyConfiguration(new IdentityUserLoginConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserRoleConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserTokenConfig());
    }
}

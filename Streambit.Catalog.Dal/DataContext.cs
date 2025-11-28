using Catalog.Infrastucture.Context.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Streambit.Catalog.Dal.Configurations;
using Streambit.Catalog.Domain.Aggregates.GenreAggregate;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;
using Streambit.Catalog.Domain.Aggregates.MovieAggregate;

namespace Streambit.Catalog.Dal;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<Language>  Languages { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MovieConfig());
        modelBuilder.ApplyConfiguration(new MovieGenreConfig());
        modelBuilder.ApplyConfiguration(new LanguageConfig());
        modelBuilder.ApplyConfiguration(new GenreConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserLoginConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserRoleConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserTokenConfig());
    }
}
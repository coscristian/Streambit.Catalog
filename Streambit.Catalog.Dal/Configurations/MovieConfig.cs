using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Catalog.Domain.Aggregates.MovieAggregate;

namespace Streambit.Catalog.Dal.Configurations;

internal class MovieConfig : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(m => m.OriginalTitle)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(m => m.OriginalLanguage)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(m => m.Overview)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(m => m.Popularity)
            .HasPrecision(10, 4);

        builder.Property(m => m.Budget)
            .HasPrecision(14, 2);

        builder.Property(m => m.VoteAverage)
            .HasPrecision(3, 1);

        builder.Property(m => m.TagLine)
            .HasMaxLength(1000);

        builder.Property(m => m.BackdropPath)
            .HasMaxLength(1000);

        builder.Property(m => m.CreatedDate)
            .IsRequired();

        builder.Property(m => m.LastModified)
            .IsRequired();

        builder.HasIndex(m => m.Title);
        builder.HasIndex(m => m.OriginalLanguage);
        builder.HasIndex(m => m.Popularity);
        builder.HasIndex(m => m.ReleaseDate);
        
        // MovieGenres
        builder.HasMany(m => m.MovieGenres)
            .WithOne(mg => mg.Movie)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // MovieProviders
        builder.HasMany(m => m.MovieProviders)
            .WithOne(mp => mp.Movie)
            .HasForeignKey(mp => mp.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // MovieImages
        builder.HasMany(m => m.MovieImages)
            .WithOne(mi => mi.Movie)
            .HasForeignKey(mi => mi.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
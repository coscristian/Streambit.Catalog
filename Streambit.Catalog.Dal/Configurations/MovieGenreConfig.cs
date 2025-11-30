using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Catalog.Domain.Aggregates.MovieAggregate;

namespace Streambit.Catalog.Dal.Configurations;

internal class MovieGenreConfig : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.ToTable("MovieGenres");
        builder.HasKey(movieGenre => new { movieGenre.MovieId, movieGenre.GenreId });

        // Movie Relation
        builder.HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        // Genre Relation
        builder.HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenres)
            .HasForeignKey(mg => mg.GenreId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
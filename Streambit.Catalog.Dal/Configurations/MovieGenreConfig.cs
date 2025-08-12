using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Catalog.Domain.Aggregates.MovieAggregate;

namespace Streambit.Catalog.Dal.Configurations;

internal class MovieGenreConfig : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.HasKey(movieGenre => new { movieGenre.MovieId, movieGenre.GenreId });
    }
}
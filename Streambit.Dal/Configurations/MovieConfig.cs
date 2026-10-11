using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Dal.Configurations;

internal class MovieConfig : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");
        builder.HasKey(movie => movie.Id);

        builder.Property(movie => movie.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(movie => movie.Overview)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(movie => movie.Rating)
            .HasPrecision(3, 1);

        builder.Property(movie => movie.PosterUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(movie => movie.BackdropUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(movie => movie.OriginalTitle)
            .IsRequired()
            .HasMaxLength(500)
            .HasDefaultValue(string.Empty);

        builder.Property(movie => movie.OriginalLanguage)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(string.Empty);

        builder.Property(movie => movie.ReleaseDate)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(string.Empty);

        builder.Property(movie => movie.Popularity)
            .IsRequired()
            .HasDefaultValue(0d);

        builder.Property(movie => movie.VoteCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(movie => movie.Adult)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(movie => movie.Video)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(movie => movie.PosterPath)
            .IsRequired()
            .HasMaxLength(1000)
            .HasDefaultValue(string.Empty);

        builder.Property(movie => movie.BackdropPath)
            .IsRequired()
            .HasMaxLength(1000)
            .HasDefaultValue(string.Empty);

        builder.Property(movie => movie.Format)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(movie => movie.Availability)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(movie => movie.CreatedDate)
            .IsRequired();

        builder.Property(movie => movie.LastModified)
            .IsRequired();

        builder.HasIndex(movie => movie.Title);
        builder.HasIndex(movie => movie.Year);
        builder.HasIndex(movie => movie.Format);
        builder.HasIndex(movie => movie.Availability);
        builder.HasIndex(movie => movie.IsFeatured);
        builder.HasIndex(movie => movie.Popularity);

        builder.HasMany(movie => movie.MovieGenres)
            .WithOne(movieGenre => movieGenre.Movie)
            .HasForeignKey(movieGenre => movieGenre.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(movie => movie.Credits)
            .WithOne(credit => credit.Movie)
            .HasForeignKey(credit => credit.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

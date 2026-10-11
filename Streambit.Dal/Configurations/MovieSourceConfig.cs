using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Dal.Configurations;

internal class MovieSourceConfig : IEntityTypeConfiguration<MovieSource>
{
    public void Configure(EntityTypeBuilder<MovieSource> builder)
    {
        builder.ToTable("MovieSources");
        builder.HasKey(link => new { link.MovieId, link.SourceId });

        builder.Property(link => link.ExternalId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(link => link.Movie)
            .WithMany(movie => movie.MovieSources)
            .HasForeignKey(link => link.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(link => link.Source)
            .WithMany()
            .HasForeignKey(link => link.SourceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(link => new { link.SourceId, link.ExternalId })
            .IsUnique();
    }
}

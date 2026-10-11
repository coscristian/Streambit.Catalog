using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.GenreAggregate;

namespace Streambit.Dal.Configurations;

internal class GenreSourceConfig : IEntityTypeConfiguration<GenreSource>
{
    public void Configure(EntityTypeBuilder<GenreSource> builder)
    {
        builder.ToTable("GenreSources");
        builder.HasKey(link => new { link.GenreId, link.SourceId });

        builder.Property(link => link.ExternalId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(link => link.Genre)
            .WithMany(genre => genre.GenreSources)
            .HasForeignKey(link => link.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(link => link.Source)
            .WithMany()
            .HasForeignKey(link => link.SourceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(link => new { link.SourceId, link.ExternalId })
            .IsUnique();
    }
}

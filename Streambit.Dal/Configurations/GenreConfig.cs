using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.GenreAggregate;

namespace Streambit.Dal.Configurations;

internal class GenreConfig : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");
        builder.HasKey(genre => genre.Id);

        builder.Property(genre => genre.Id)
            .ValueGeneratedOnAdd();

        builder.Property(genre => genre.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(genre => genre.ImageUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasIndex(genre => genre.Name)
            .IsUnique();
    }
}

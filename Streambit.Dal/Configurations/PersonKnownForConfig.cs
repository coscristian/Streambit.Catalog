using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.PersonAggregate;

namespace Streambit.Dal.Configurations;

internal class PersonKnownForConfig : IEntityTypeConfiguration<PersonKnownFor>
{
    public void Configure(EntityTypeBuilder<PersonKnownFor> builder)
    {
        builder.ToTable("PersonKnownFor");
        builder.HasKey(work => work.Id);

        builder.Property(work => work.ExternalId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(work => work.MediaType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(work => work.BackdropPath)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(work => work.PosterPath)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(work => work.OriginalLanguage)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(work => work.OriginalTitle)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(work => work.OriginalName)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(work => work.Overview)
            .IsRequired();

        builder.Property(work => work.ReleaseDate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(work => work.FirstAirDate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(work => work.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(work => work.Name)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(work => work.Person)
            .WithMany(person => person.KnownFor)
            .HasForeignKey(work => work.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(work => new { work.PersonId, work.SortOrder });
        builder.HasIndex(work => new { work.PersonId, work.MediaType, work.ExternalId })
            .IsUnique();
    }
}

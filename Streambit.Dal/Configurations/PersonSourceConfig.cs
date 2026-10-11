using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.PersonAggregate;

namespace Streambit.Dal.Configurations;

internal class PersonSourceConfig : IEntityTypeConfiguration<PersonSource>
{
    public void Configure(EntityTypeBuilder<PersonSource> builder)
    {
        builder.ToTable("PersonSources");
        builder.HasKey(link => new { link.PersonId, link.SourceId });

        builder.Property(link => link.ExternalId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(link => link.Person)
            .WithMany(person => person.PersonSources)
            .HasForeignKey(link => link.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(link => link.Source)
            .WithMany()
            .HasForeignKey(link => link.SourceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(link => new { link.SourceId, link.ExternalId })
            .IsUnique();
    }
}

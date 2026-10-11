using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.PersonAggregate;

namespace Streambit.Dal.Configurations;

internal class PersonConfig : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("People");
        builder.HasKey(person => person.Id);

        builder.Property(person => person.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(person => person.PhotoUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(person => person.ProfilePath)
            .IsRequired()
            .HasMaxLength(1000)
            .HasDefaultValue(string.Empty);

        builder.Property(person => person.Adult)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(person => person.Gender)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(person => person.KnownForDepartment)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue(string.Empty);

        builder.Property(person => person.Popularity)
            .IsRequired()
            .HasDefaultValue(0d);

        builder.Property(person => person.CreatedDate)
            .IsRequired();

        builder.Property(person => person.LastModified)
            .IsRequired();

        builder.HasIndex(person => person.Name);

        builder.HasMany(person => person.Credits)
            .WithOne(credit => credit.Person)
            .HasForeignKey(credit => credit.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

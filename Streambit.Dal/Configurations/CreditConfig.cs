using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Dal.Configurations;

internal class CreditConfig : IEntityTypeConfiguration<Credit>
{
    public void Configure(EntityTypeBuilder<Credit> builder)
    {
        builder.ToTable("Credits");
        builder.HasKey(credit => new { credit.MovieId, credit.PersonId });

        builder.Property(credit => credit.Role)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(credit => credit.Movie)
            .WithMany(movie => movie.Credits)
            .HasForeignKey(credit => credit.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(credit => credit.Person)
            .WithMany(person => person.Credits)
            .HasForeignKey(credit => credit.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(credit => credit.PersonId);
    }
}

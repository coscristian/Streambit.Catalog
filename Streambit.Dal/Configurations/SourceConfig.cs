using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.SourceAggregate;

namespace Streambit.Dal.Configurations;

internal class SourceConfig : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.ToTable("Sources");
        builder.HasKey(source => source.Id);

        builder.Property(source => source.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(source => source.CreatedDate)
            .IsRequired();

        builder.Property(source => source.LastModified)
            .IsRequired();

        builder.HasIndex(source => source.Name)
            .IsUnique();
    }
}

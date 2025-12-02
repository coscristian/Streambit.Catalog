using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Catalog.Domain.Aggregates.ProviderAggregate;

namespace Streambit.Catalog.Dal.Configurations;

public class ProviderConfig : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable("Providers");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Code)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.ApiBaseUrl)
            .HasMaxLength(1000);

        builder.Property(p => p.WebBaseUrl)
            .HasMaxLength(1000);

        builder.Property(p => p.SupportsRatings);
        builder.Property(p => p.SupportsImages);
        builder.Property(p => p.SupportsCast);
        builder.Property(p => p.SupportsKeywords);

        builder.HasIndex(p => p.Code).IsUnique();
    }
}
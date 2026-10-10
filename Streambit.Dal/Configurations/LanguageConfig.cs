using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Dal.Configurations;

internal class LanguageConfig : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(l => l.LanguageId);
        
        builder.Property(l => l.LanguageId)
            .ValueGeneratedOnAdd();
    }
}
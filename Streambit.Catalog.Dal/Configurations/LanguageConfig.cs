using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Dal.Configurations;

internal class LanguageConfig : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(x => x.LanguageId);
    }
}
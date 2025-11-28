using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Catalog.Domain.Aggregates.MovieAggregate;

namespace Streambit.Catalog.Dal.Configurations;

internal class MovieConfig : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.MovieId);
        builder.Property(m => m.Title).IsRequired();
        builder.Property(m => m.OriginalTitle).IsRequired();
        builder.Property(m =>  m.Overview).IsRequired();
        builder.Property(m =>  m.Popularity).IsRequired();
        
        // Referencias
        
    }
}
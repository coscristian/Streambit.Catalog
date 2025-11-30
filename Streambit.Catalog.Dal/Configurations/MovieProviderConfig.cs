using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Catalog.Domain.Aggregates.MovieAggregate;

namespace Streambit.Catalog.Dal.Configurations;

public class MovieProviderConfig : IEntityTypeConfiguration<MovieProvider>
{
    public void Configure(EntityTypeBuilder<MovieProvider> builder)
    {
        builder.ToTable("MovieProviders");
        builder.HasKey(mp => new { mp.MovieId, mp.ProviderId });
        
        builder.Property(mp => mp.ExternalId).IsRequired();
        
        // Movie Relation
        builder.HasOne(mp => mp.Movie)
            .WithMany(m => m.MovieProviders)
            .HasForeignKey(mp => mp.MovieId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        // Provider Relation
        builder.HasOne(mp => mp.Provider)
            .WithMany()
            .HasForeignKey(mp => mp.ProviderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
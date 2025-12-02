using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Catalog.Domain.Aggregates.MovieAggregate;

namespace Streambit.Catalog.Dal.Configurations;

public class MovieImageConfig : IEntityTypeConfiguration<MovieImage> 
{
    public void Configure(EntityTypeBuilder<MovieImage> builder)
    {
        builder.ToTable("MovieImages");

        builder.HasKey(mi => mi.Id);
        
        builder.Property(mi => mi.Id)
            .ValueGeneratedOnAdd();

        builder.Property(mi => mi.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(mi => mi.Url)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(mi => mi.ProviderId);

        builder.HasOne(mi => mi.Movie)
            .WithMany(m => m.MovieImages)
            .HasForeignKey(mi => mi.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mi => mi.Provider)
            .WithMany()
            .HasForeignKey(mi => mi.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
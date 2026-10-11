using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streambit.Domain.Aggregates.SyncAggregate;

namespace Streambit.Dal.Configurations;

internal class SyncLogConfig : IEntityTypeConfiguration<SyncLog>
{
    public void Configure(EntityTypeBuilder<SyncLog> builder)
    {
        builder.ToTable("SyncLogs");
        builder.HasKey(log => log.Id);

        builder.Property(log => log.Entity)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(log => log.Trigger)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(log => log.Operation)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(log => log.Attempt)
            .IsRequired();

        builder.Property(log => log.HttpMethod)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(log => log.RequestUrl)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(log => log.ErrorMessage)
            .IsRequired();

        builder.Property(log => log.ResponseBody)
            .IsRequired();

        builder.Property(log => log.StartedAt)
            .IsRequired();

        builder.Property(log => log.FinishedAt)
            .IsRequired();

        builder.Property(log => log.DurationMs)
            .IsRequired();

        builder.HasIndex(log => log.StartedAt);
        builder.HasIndex(log => log.RunId);
        builder.HasIndex(log => log.Succeeded);
    }
}

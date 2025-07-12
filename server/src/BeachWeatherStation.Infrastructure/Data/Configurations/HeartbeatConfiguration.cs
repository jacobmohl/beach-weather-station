using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BeachWeatherStation.Domain.Entities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace BeachWeatherStation.Infrastructure.Data.Configurations;

public class HeartbeatConfiguration : IEntityTypeConfiguration<Heartbeat>
{
    public void Configure(EntityTypeBuilder<Heartbeat> builder)
    {
        builder.ToContainer("HeartbeatsProd")
            .Property(e => e.Id)
            .HasValueGenerator<GuidValueGenerator>();

        builder.HasKey(e => e.Id);

        builder.Property<string>("CreatedAtYearMonth")
            .HasValueGenerator<YearMonthValueGenerator>();

        builder.HasPartitionKey(e => new
        {
            e.DeviceId,
        });

        builder.HasDefaultTimeToLive(7 * 24 * 60 * 60); // 7 days in seconds

        // builder.HasIndex(e => e.DeviceId);
        // builder.HasIndex(e => e.CreatedAt);
    }
}

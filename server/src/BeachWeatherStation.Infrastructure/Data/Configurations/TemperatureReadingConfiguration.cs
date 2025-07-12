using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BeachWeatherStation.Domain.Entities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace BeachWeatherStation.Infrastructure.Data.Configurations;

public class TemperatureReadingConfiguration : IEntityTypeConfiguration<TemperatureReading>
{
    public void Configure(EntityTypeBuilder<TemperatureReading> builder)
    {
        builder.ToContainer("TemperatureReadingsTest")
            .Property(e => e.Id)
            .HasValueGenerator<GuidValueGenerator>();

        builder.HasKey(e => e.Id);

        builder.Property<string>("CreatedAtYearMonth")
            .HasValueGenerator<YearMonthValueGenerator>();

        builder.HasPartitionKey(e => new
        {
            e.DeviceId,
        });

        builder.HasIndex(e => e.DeviceId);
        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => e.Temperature);
    }
}

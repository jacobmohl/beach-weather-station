using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Infrastructure.Data.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToContainer("DevicesTest")
            .Property(e => e.Id);

        builder.HasKey(e => e.Id);

        builder.HasPartitionKey(e => new
        {
            e.Id
        });

        builder.HasIndex(e => e.Id);
    }
}

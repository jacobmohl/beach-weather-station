using Microsoft.EntityFrameworkCore;
using BeachWeatherStation.Domain.Entities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace BeachWeatherStation.Infrastructure.Data;

public class BeachWeatherStationDbContext : DbContext
{
    public BeachWeatherStationDbContext(DbContextOptions<BeachWeatherStationDbContext> options)
        : base(options) { }

    // DbSets map to Cosmos DB containers
    public DbSet<BatteryChange> BatteryChanges { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Heartbeat> Heartbeats { get; set; }
    public DbSet<TemperatureReading> TemperatureReadings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BeachWeatherStationDbContext).Assembly);

        // Seed the database with a default device
        modelBuilder.Entity<Device>().HasData(
            new Device
            {
                Id = "Sensor1",
                Name = "Fynshoved",
                Status = DeviceStatus.Online
            });

        base.OnModelCreating(modelBuilder);
    }
}

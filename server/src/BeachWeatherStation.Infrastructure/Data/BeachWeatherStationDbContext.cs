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

        modelBuilder.Entity<BatteryChange>()
            .ToContainer("BatteryChangesTest")
            .Property(e => e.Id)
                .HasValueGenerator<GuidValueGenerator>();  
        modelBuilder.Entity<BatteryChange>()                          
            .HasKey(e => e.Id);

        modelBuilder.Entity<Device>()
            .ToContainer("DevicesTest")
            .Property(e => e.Id)
                .HasValueGenerator<GuidValueGenerator>(); 
        modelBuilder.Entity<Device>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Heartbeat>()
            .ToContainer("HeartbeatsTest")
            .Property(e => e.Id)
                .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<Heartbeat>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<TemperatureReading>()
            .ToContainer("TemperatureReadingsTest")
            .Property(e => e.Id)
                .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<TemperatureReading>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<TemperatureReading>()
            .Property<string>("CreatedAtYearMonth")
                .HasValueGenerator<YearMonthValueGenerator>();
        modelBuilder.Entity<TemperatureReading>()
            .HasPartitionKey(e => new object[] {
                e.DeviceId,
                EF.Property<string>(e, "CreatedAtYearMonth"),
                e.CreatedAt
            });

        // Seed the database with a default device
        modelBuilder.Entity<Device>().HasData(
            new Device
            {
                Id = "Sensor1",
                Name = "Fynshoved",
                Status = DeviceStatus.Online
            });
        //base.OnModelCreating(modelBuilder);
    }
}

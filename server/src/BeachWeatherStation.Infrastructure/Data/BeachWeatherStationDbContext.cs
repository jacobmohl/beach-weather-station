using Microsoft.EntityFrameworkCore;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Infrastructure.Data
{
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
            modelBuilder.HasDefaultContainer("BeachWeatherStation");
            modelBuilder.Entity<BatteryChange>();
            modelBuilder.Entity<Device>();
            modelBuilder.Entity<Heartbeat>();
            modelBuilder.Entity<TemperatureReading>();
        }
    }
}

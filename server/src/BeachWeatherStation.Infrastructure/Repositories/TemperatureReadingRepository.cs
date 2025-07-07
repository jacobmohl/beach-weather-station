using BeachWeatherStation.Domain.Entities;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeachWeatherStation.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing temperature readings in the database.
    /// </summary>
    public class TemperatureReadingRepository : ITemperatureReadingRepository
    {
        // EF Core DbContext for data access
        private readonly BeachWeatherStationDbContext _dbContext;
        /// <summary>
        /// Constructor with dependency injection for DbContext.
        /// </summary>
        public TemperatureReadingRepository(BeachWeatherStationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get a temperature reading by its string ID.
        /// </summary>
        public TemperatureReading GetTemperatureReadingById(Guid temperatuReadingId)
        {
            var doc = _dbContext.TemperatureReadings.AsNoTracking().FirstOrDefault(x => x.Id == temperatuReadingId);
            if (doc == null) return null!;
            return doc;
        }

        /// <summary>
        /// Get all temperature readings for a specific device.
        /// </summary>
        public IEnumerable<TemperatureReading> GetReadingsByDeviceId(Guid deviceId)
        {
            return _dbContext.TemperatureReadings.AsNoTracking().Where(x => x.Id == deviceId)
                .ToList();
        }

        /// <summary>
        /// Add a new temperature reading to the database.
        /// </summary>
        public void AddReading(TemperatureReading temperatureReading)
        {
            _dbContext.TemperatureReadings.Add(temperatureReading);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update an existing temperature reading in the database.
        /// </summary>
        public void UpdateReading(TemperatureReading temperatureReading)
        {
            var doc = _dbContext.TemperatureReadings.FirstOrDefault(x => x.Id == temperatureReading.Id);
            if (doc != null)
            {
                doc.DeviceId = temperatureReading.DeviceId;
                doc.CreatedAt = temperatureReading.CreatedAt;
                doc.Temperature = temperatureReading.Temperature;
                _dbContext.TemperatureReadings.Update(doc);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteReading(Guid temperatureReadingId)
        {
            var doc = _dbContext.TemperatureReadings.FirstOrDefault(x => x.Id == temperatureReadingId);
            if (doc != null)
            {
                _dbContext.TemperatureReadings.Remove(doc);
                _dbContext.SaveChanges();
            }
        }
    }
}

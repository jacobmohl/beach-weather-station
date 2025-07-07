using BeachWeatherStation.Domain.Entities;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeachWeatherStation.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing heartbeat events in the database.
    /// </summary>
    public class HeartbeatRepository : IHeartbeatRepository
    {
        // EF Core DbContext for data access
        private readonly BeachWeatherStationDbContext _dbContext;
        /// <summary>
        /// Constructor with dependency injection for DbContext.
        /// </summary>
        public HeartbeatRepository(BeachWeatherStationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get a heartbeat event by its string ID.
        /// </summary>
        public Heartbeat GetHeartbeatById(Guid heartbeatId)
        {
            var doc = _dbContext.Heartbeats.AsNoTracking().FirstOrDefault(x => x.Id == heartbeatId);
            if (doc == null) return null!;
            return doc;
        }

        /// <summary>
        /// Get all heartbeat events for a specific device.
        /// </summary>
        public IEnumerable<Heartbeat> GetHeartbeatsByDeviceId(Guid deviceId)
        {
            return _dbContext.Heartbeats.AsNoTracking().Where(x => x.Id == deviceId)
                .ToList();
        }

        /// <summary>
        /// Add a new heartbeat event to the database.
        /// </summary>
        public void AddHeartbeat(Heartbeat heartbeat)
        {
            _dbContext.Heartbeats.Add(heartbeat);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update an existing heartbeat event in the database.
        /// </summary>
        public void UpdateHeartbeat(Heartbeat heartbeat)
        {
            var doc = _dbContext.Heartbeats.FirstOrDefault(x => x.Id == heartbeat.Id);
            if (doc != null)
            {
                doc.DeviceId = heartbeat.DeviceId;
                doc.CreatedAt = heartbeat.CreatedAt;
                _dbContext.Heartbeats.Update(doc);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteHeartbeat(Guid heartbeatId)
        {
            var doc = _dbContext.Heartbeats.FirstOrDefault(x => x.Id == heartbeatId);
            if (doc != null)
            {
                _dbContext.Heartbeats.Remove(doc);
                _dbContext.SaveChanges();
            }
        }
    }
}

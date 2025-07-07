using BeachWeatherStation.Domain.Entities;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeachWeatherStation.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing device entities in the database.
    /// </summary>
    public class DeviceRepository : IDeviceRepository
    {
        // EF Core DbContext for data access
        private readonly BeachWeatherStationDbContext _dbContext;
        /// <summary>
        /// Constructor with dependency injection for DbContext.
        /// </summary>
        public DeviceRepository(BeachWeatherStationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get a device by its string ID.
        /// </summary>
        public Device GetDeviceById(Guid deviceId)
        {
            var doc = _dbContext.Devices.AsNoTracking().FirstOrDefault(x => x.Id == deviceId);
            if (doc == null) return null!;
            return doc;
        }

        /// <summary>
        /// Get all devices in the database.
        /// </summary>
        public IEnumerable<Device> GetAllDevices()
        {
            return _dbContext.Devices
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Add a new device to the database.
        /// </summary>
        public void AddDevice(Device device)
        {
            _dbContext.Devices.Add(device);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update an existing device in the database.
        /// </summary>
        public void UpdateDevice(Device device)
        {
            var doc = _dbContext.Devices.FirstOrDefault(x => x.Id == device.Id);
            if (doc != null)
            {
                doc.Name = device.Name;
                doc.Status = device.Status;
                _dbContext.Devices.Update(doc);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteDevice(Guid deviceId)
        {
            var doc = _dbContext.Devices.FirstOrDefault(x => x.Id == deviceId);
            if (doc != null)
            {
                _dbContext.Devices.Remove(doc);
                _dbContext.SaveChanges();
            }
        }
    }
}

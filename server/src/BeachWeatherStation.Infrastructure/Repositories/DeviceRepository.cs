using BeachWeatherStation.Domain.Entities;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeachWeatherStation.Infrastructure.Repositories;

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
    public async Task<Device?> GetDeviceByIdAsync(string deviceId)
    {
        var doc = await _dbContext.Devices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == deviceId);
        return doc;
    }

    /// <summary>
    /// Get all devices in the database.
    /// </summary>
    public async Task<IEnumerable<Device>> GetAllDevicesAsync()
    {
        return await _dbContext.Devices
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Add a new device to the database.
    /// </summary>
    public async Task AddDeviceAsync(Device device)
    {
        await _dbContext.Devices.AddAsync(device);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Update an existing device in the database.
    /// </summary>
    public async Task UpdateDeviceAsync(Device device)
    {
        var doc = await _dbContext.Devices.FirstOrDefaultAsync(x => x.Id == device.Id);
        if (doc != null)
        {
            doc.Name = device.Name;
            doc.Status = device.Status;
            _dbContext.Devices.Update(doc);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteDeviceAsync(string deviceId)
    {
        var doc = await _dbContext.Devices.FirstOrDefaultAsync(x => x.Id == deviceId);
        if (doc != null)
        {
            _dbContext.Devices.Remove(doc);
            await _dbContext.SaveChangesAsync();
        }
    }
}

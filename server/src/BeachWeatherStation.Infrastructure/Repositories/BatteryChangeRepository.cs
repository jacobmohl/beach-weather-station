using BeachWeatherStation.Domain.Entities;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeachWeatherStation.Infrastructure.Repositories;

/// <summary>
/// Repository for managing battery change events in the database.
/// </summary>
public class BatteryChangeRepository : IBatteryChangeRepository
{
    // EF Core DbContext for data access
    private readonly BeachWeatherStationDbContext _dbContext;
    /// <summary>
    /// Constructor with dependency injection for DbContext.
    /// </summary>
    public BatteryChangeRepository(BeachWeatherStationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Get a battery change event by its string ID.
    /// </summary>
    public async Task<BatteryChange?> GetBatteryChangeByIdAsync(Guid batteryChangeId)
    {
        var doc = await _dbContext.BatteryChanges.AsNoTracking().FirstOrDefaultAsync(x => x.Id == batteryChangeId);
        return doc;
    }

    /// <summary>
    /// Get all battery change events for a specific device.
    /// </summary>
    public async Task<IEnumerable<BatteryChange>> GetBatteryChangesByDeviceIdAsync(string deviceId)
    {
        
        
        return await _dbContext.BatteryChanges
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .ToListAsync();
    }

    /// <summary>
    /// Add a new battery change event to the database.
    /// </summary>
    public async Task AddBatteryChangeAsync(BatteryChange batteryChange)
    {
        await _dbContext.BatteryChanges.AddAsync(batteryChange);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Update an existing battery change event in the database.
    /// </summary>
    public async Task UpdateBatteryChangeAsync(BatteryChange batteryChange)
    {
        var doc = await _dbContext.BatteryChanges.FirstOrDefaultAsync(x => x.Id == batteryChange.Id);
        if (doc != null)
        {
            doc.DeviceId = batteryChange.DeviceId;
            doc.CreatedAt = batteryChange.CreatedAt;
            _dbContext.BatteryChanges.Update(doc);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteBatteryChangeAsync(Guid batteryChangeId)
    {
        var doc = await _dbContext.BatteryChanges.FirstOrDefaultAsync(x => x.Id == batteryChangeId);
        if (doc != null)
        {
            _dbContext.BatteryChanges.Remove(doc);
            await _dbContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Retrieves the latest battery change for a specific device.
    /// </summary>
    public async Task<BatteryChange?> GetLatestBatteryChangeAsync(string deviceId)
    {
        return await _dbContext.BatteryChanges
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves battery changes from the last 24 hours for a specific device.
    /// </summary>
    public async Task<IEnumerable<BatteryChange>> GetBatteryChangesLast24hAsync(string deviceId)
    {
        await _dbContext.Database.EnsureCreatedAsync();
        
        var since = DateTime.UtcNow.AddHours(-24);
        return await _dbContext.BatteryChanges
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId && x.CreatedAt >= since)
            .ToListAsync();
    }
}

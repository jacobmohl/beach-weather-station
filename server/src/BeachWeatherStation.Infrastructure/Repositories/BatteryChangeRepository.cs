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
    public BatteryChange GetBatteryChangeById(Guid batteryChangeId)
    {
        var doc = _dbContext.BatteryChanges.AsNoTracking().FirstOrDefault(x => x.Id == batteryChangeId);
        if (doc == null) return null!;
        return doc;
    }

    /// <summary>
    /// Get all battery change events for a specific device.
    /// </summary>
    public IEnumerable<BatteryChange> GetBatteryChangesByDeviceId(Guid deviceId)
    {
        return _dbContext.BatteryChanges
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .ToList();
    }

    /// <summary>
    /// Add a new battery change event to the database.
    /// </summary>
    public void AddBatteryChange(BatteryChange batteryChange)
    {
        _dbContext.BatteryChanges.Add(batteryChange);
        _dbContext.SaveChanges();
    }

    /// <summary>
    /// Update an existing battery change event in the database.
    /// </summary>
    public void UpdateBatteryChange(BatteryChange batteryChange)
    {
        var doc = _dbContext.BatteryChanges.FirstOrDefault(x => x.Id == batteryChange.Id);
        if (doc != null)
        {
            doc.DeviceId = batteryChange.DeviceId;
            doc.CreatedAt = batteryChange.CreatedAt;
            _dbContext.BatteryChanges.Update(doc);
            _dbContext.SaveChanges();
        }
    }

    public void DeleteBatteryChange(Guid batteryChangeId)
    {
        var doc = _dbContext.BatteryChanges.FirstOrDefault(x => x.Id == batteryChangeId);
        if (doc != null)
        {
            _dbContext.BatteryChanges.Remove(doc);
            _dbContext.SaveChanges();
        }
    }
}

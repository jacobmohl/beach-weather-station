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
    /// <summary>
    /// Get the latest heartbeat event for a specific device.
    /// </summary>
    public async Task<Heartbeat?> GetLatestHeartbeatAsync(string deviceId)
    {
        return await _dbContext.Heartbeats.AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get all heartbeat events for a device in the last 24 hours.
    /// </summary>
    public async Task<IEnumerable<Heartbeat>> GetHeartbeatsLast24hAsync(string deviceId)
    {
        var since = DateTimeOffset.UtcNow.AddHours(-24);
        return await _dbContext.Heartbeats.AsNoTracking()
            .Where(x => x.DeviceId == deviceId && x.CreatedAt >= since)
            .ToListAsync();
    }
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
    public async Task<Heartbeat?> GetHeartbeatByIdAsync(Guid heartbeatId)
    {
        var doc = await _dbContext.Heartbeats.AsNoTracking().FirstOrDefaultAsync(x => x.Id == heartbeatId);
        return doc;
    }

    /// <summary>
    /// Get all heartbeat events for a specific device.
    /// </summary>
    public async Task<IEnumerable<Heartbeat>> GetHeartbeatsByDeviceIdAsync(string deviceId)
    {
        return await _dbContext.Heartbeats.AsNoTracking().Where(x => x.DeviceId == deviceId)
            .ToListAsync();
    }
    /// <summary>
    /// Get all heartbeat events in the database.
    /// </summary>
    public async Task<IEnumerable<Heartbeat>> GetAllHeartbeatsAsync()
    {
        return await _dbContext.Heartbeats.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Check if a heartbeat exists by its ID.
    /// </summary>
    public async Task<bool> HeartbeatExistsAsync(Guid heartbeatId)
    {
        return await _dbContext.Heartbeats.AsNoTracking().AnyAsync(x => x.Id == heartbeatId);
    }

    /// <summary>
    /// Add a new heartbeat event to the database.
    /// </summary>
    public async Task AddHeartbeatAsync(Heartbeat heartbeat)
    {
        await _dbContext.Heartbeats.AddAsync(heartbeat);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Update an existing heartbeat event in the database.
    /// </summary>
    public async Task UpdateHeartbeatAsync(Heartbeat heartbeat)
    {
        var doc = await _dbContext.Heartbeats.FirstOrDefaultAsync(x => x.Id == heartbeat.Id);
        if (doc != null)
        {
            doc.DeviceId = heartbeat.DeviceId;
            doc.CreatedAt = heartbeat.CreatedAt;
            _dbContext.Heartbeats.Update(doc);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteHeartbeatAsync(Guid heartbeatId)
    {
        var doc = await _dbContext.Heartbeats.FirstOrDefaultAsync(x => x.Id == heartbeatId);
        if (doc != null)
        {
            _dbContext.Heartbeats.Remove(doc);
            await _dbContext.SaveChangesAsync();
        }
    }
}
}

using System.Threading.Tasks;
using BeachWeatherStation.Domain.Entities;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeachWeatherStation.Infrastructure.Repositories;

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
    public async Task<TemperatureReading?> GetTemperatureReadingByIdAsync(Guid temperatuReadingId)
    {
        var doc = await _dbContext.TemperatureReadings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == temperatuReadingId);
        return doc;
    }

    /// <summary>
    /// Get all temperature readings for a specific device.
    /// </summary>
    public async Task<IEnumerable<TemperatureReading>> GetReadingsByDeviceIdAsync(string deviceId)
    {
        return await _dbContext.TemperatureReadings.AsNoTracking().Where(x => x.DeviceId == deviceId)
            .ToListAsync();
    }

    /// <summary>
    /// Add a new temperature reading to the database.
    /// </summary>
    public async Task AddReadingAsync(TemperatureReading temperatureReading)
    {
        await _dbContext.TemperatureReadings.AddAsync(temperatureReading);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Update an existing temperature reading in the database.
    /// </summary>
    public async Task UpdateReadingAsync(TemperatureReading temperatureReading)
    {
        var doc = await _dbContext.TemperatureReadings.FirstOrDefaultAsync(x => x.Id == temperatureReading.Id);
        if (doc != null)
        {
            doc.DeviceId = temperatureReading.DeviceId;
            doc.CreatedAt = temperatureReading.CreatedAt;
            doc.Temperature = temperatureReading.Temperature;
            _dbContext.TemperatureReadings.Update(doc);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteReadingAsync(Guid temperatureReadingId)
    {
        var doc = await _dbContext.TemperatureReadings.FirstOrDefaultAsync(x => x.Id == temperatureReadingId);
        if (doc != null)
        {
            _dbContext.TemperatureReadings.Remove(doc);
            await _dbContext.SaveChangesAsync();
        }
    }


    public async Task<TemperatureReading?> GetLatestReadingAsync(string deviceId)
    {
        var reading = await _dbContext.TemperatureReadings
            .WithPartitionKey(deviceId)
            .AsNoTracking()
            .OrderByDescending(x =>  x.CreatedAt)
            .FirstOrDefaultAsync();

        return reading;
    }

    public async Task<(IEnumerable<TemperatureReading> Readings, TemperatureReading? Highest, TemperatureReading? Lowest)> GetReadingsForLast24hWithMinMaxAsync(string deviceId)
    {
        var since = DateTimeOffset.UtcNow.AddHours(-24);
        
        var readings = await _dbContext.TemperatureReadings
            .WithPartitionKey(deviceId)
            .Where(x => x.DeviceId == deviceId && x.CreatedAt >= since)
            .OrderBy(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync();

        var highest = readings.OrderByDescending(x => x.Temperature).FirstOrDefault();
        var lowest = readings.OrderBy(x => x.Temperature).FirstOrDefault();
        return (readings, highest, lowest);
    }

    public async Task<IEnumerable<DailyTemperatureStats>> GetDailyStatsForLast30DaysAsync(string deviceId)
    {
        var since = DateTimeOffset.UtcNow.Date.AddDays(-30);
        var readings = await _dbContext.TemperatureReadings
            .WithPartitionKey(deviceId)
            .Where(x => x.DeviceId == deviceId && x.CreatedAt >= since)
            .OrderBy(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync();

        var stats = readings
            .GroupBy(r => TimeZoneInfo.ConvertTimeFromUtc(r.CreatedAt.DateTime, TimeZoneInfo.Local).Date)
            .Select(g => new DailyTemperatureStats
            {
                Date = g.Key,
                Average = g.Average(r => r.Temperature),
                Minimum = g.Min(r => r.Temperature),
                Maximum = g.Max(r => r.Temperature)
            })
            .OrderByDescending(s => s.Date)
            .ToList();
        return stats;
    }
}

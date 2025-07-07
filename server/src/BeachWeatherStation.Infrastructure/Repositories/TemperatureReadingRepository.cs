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


    public TemperatureReading? GetLatest(Guid deviceId)
    {
        var since = DateTime.UtcNow.AddHours(-24);
        var reading = _dbContext.TemperatureReadings.AsNoTracking()
               .OrderByDescending(x =>  x.CreatedAt)
                .FirstOrDefault();

        return reading;
    }

    public (IEnumerable<TemperatureReading> Readings, TemperatureReading? Highest, TemperatureReading? Lowest) GetReadingsForLast24hWithMinMax(Guid deviceId)
    {
        var since = DateTime.UtcNow.AddHours(-24);
        var readings = _dbContext.TemperatureReadings.AsNoTracking()
            .Where(x => x.DeviceId == deviceId && x.CreatedAt >= since)
            .OrderBy(x => x.CreatedAt)
            .ToList();
        var highest = readings.OrderByDescending(x => x.Temperature).FirstOrDefault();
        var lowest = readings.OrderBy(x => x.Temperature).FirstOrDefault();
        return (readings, highest, lowest);
    }

    public IEnumerable<DailyTemperatureStats> GetDailyStatsForLast30Days(Guid deviceId)
    {
        var since = DateTime.UtcNow.Date.AddDays(-30);
        var readings = _dbContext.TemperatureReadings.AsNoTracking()
            .Where(x => x.DeviceId == deviceId && x.CreatedAt >= since)
            .ToList();
        var stats = readings
            .GroupBy(r => r.CreatedAt.Date)
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

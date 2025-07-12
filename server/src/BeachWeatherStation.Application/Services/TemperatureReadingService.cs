using BeachWeatherStation.Application.DTOs;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Domain.Entities;
using BeachWeatherStation.Application.Validators;
using Microsoft.Extensions.Caching.Hybrid;

namespace BeachWeatherStation.Application.Services;

public class TemperatureReadingService
{
    private readonly ITemperatureReadingRepository _readingRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly TemperatureReadingValidator _validator;
    private readonly HybridCache _cache;

    public TemperatureReadingService(
        ITemperatureReadingRepository readingRepository,
        IDeviceRepository deviceRepository,
        TemperatureReadingValidator validator,
        HybridCache cache)
    {
        _readingRepository = readingRepository;
        _deviceRepository = deviceRepository;
        _validator = validator;
        _cache = cache;
    }

    public async Task<bool> IngestReadingAsync(CreateTemperatureReadingDto dto)
    {
        var validationResult = _validator.Validate(dto);
        if (!validationResult.IsValid)
            return false;

        var entity = new TemperatureReading
        {
            Id = Guid.NewGuid(),
            DeviceId = dto.DeviceId,
            CreatedAt = dto.CreatedAt,
            Temperature = dto.Temperature,
            SignalStrength = dto.SignalStrength,
        };
        await _readingRepository.AddReadingAsync(entity);

        await _cache.RemoveByTagAsync("TemperatureReadings");

        return true;
    }

    public async Task<TemperatureReading?> GetLatestReadingAsync(string deviceId)
    {
        var cacheKey = $"TemperatureReadings:LatestReading:{deviceId}";
        var cacheTags = new[] { "TemperatureReadings", "Device_" + deviceId };
        var entryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(45),
            LocalCacheExpiration = TimeSpan.FromMinutes(45)
        };

        var entity = await _cache.GetOrCreateAsync(
            cacheKey, // Unique key to the cache entry
            async cancel => await _readingRepository.GetLatestReadingAsync(deviceId),
            entryOptions,
            cacheTags
        );

        if (entity == null) return null;
        return entity;
    }

    public async Task<(IReadOnlyList<TemperatureReading> Readings, TemperatureReading? Highest, TemperatureReading? Lowest)> GetReadingsLast24hAsync(string deviceId)
    {
        var cacheKey = $"TemperatureReadings:Last24hReading:{deviceId}";
        var cacheTags = new[] { "TemperatureReadings", "Device_" + deviceId };
        var entryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(45),
            LocalCacheExpiration = TimeSpan.FromMinutes(45)
        };        

        var (readings, highest, lowest) = await _cache.GetOrCreateAsync(
            cacheKey,
            async cancel => await _readingRepository.GetReadingsForLast24hWithMinMaxAsync(deviceId),
            entryOptions,
            cacheTags
        );

        return (readings.ToList(), highest, lowest);
    }

    public async Task<IReadOnlyList<DailyTemperatureStats>> GetDailyStatsLast30DaysAsync(string deviceId)
    {

        var cacheKey = $"TemperatureReadings:DailyStatsLast30:{deviceId}";
        var cacheTags = new[] { "TemperatureReadings", "Device_" + deviceId };
        var entryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(120),
            LocalCacheExpiration = TimeSpan.FromMinutes(120)
        };  

        var stats = await _cache.GetOrCreateAsync(
            cacheKey,
            async cancel => await _readingRepository.GetDailyStatsForLast30DaysAsync(deviceId),
            entryOptions,
            cacheTags
        );        


        return stats.ToList();
    }

    public async Task<bool> DeleteReadingAsync(string id)
    {
        if (!Guid.TryParse(id, out var readingId))
        {
            return false;
        }

        await _readingRepository.DeleteReadingAsync(readingId);
        await _cache.RemoveByTagAsync("TemperatureReadings");

        return true;
    }
}

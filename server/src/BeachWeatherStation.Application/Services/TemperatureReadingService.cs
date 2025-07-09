using BeachWeatherStation.Application.DTOs;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Domain.Entities;
using BeachWeatherStation.Application.Validators;

namespace BeachWeatherStation.Application.Services;

public class TemperatureReadingService
{
    private readonly ITemperatureReadingRepository _readingRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly TemperatureReadingValidator _validator;

    public TemperatureReadingService(
        ITemperatureReadingRepository readingRepository,
        IDeviceRepository deviceRepository,
        TemperatureReadingValidator validator)
    {
        _readingRepository = readingRepository;
        _deviceRepository = deviceRepository;
        _validator = validator;
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
        return true;
    }

    public async Task<TemperatureReading?> GetLatestReadingAsync(string deviceId)
    {
        var entity = await _readingRepository.GetLatestReadingAsync(deviceId);
        if (entity == null) return null;
        return entity;
    }

    public async Task<(IReadOnlyList<TemperatureReading> Readings, TemperatureReading? Highest, TemperatureReading? Lowest)> GetReadingsLast24hAsync(string deviceId)
    {
        var (readings, highest, lowest) = await _readingRepository.GetReadingsForLast24hWithMinMaxAsync(deviceId);
        return (readings.ToList(), highest, lowest);
    }

    public async Task<IReadOnlyList<DailyTemperatureStats>> GetDailyStatsLast30DaysAsync(string deviceId)
    {
        var stats = await _readingRepository.GetDailyStatsForLast30DaysAsync(deviceId);
        return stats.ToList();
    }
}

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

    public async Task<TemperatureReadingDto?> GetLatestReadingAsync(Guid deviceId)
    {
        var entity = await _readingRepository.GetLatestReadingAsync(deviceId);
        if (entity == null) return null;
        return new TemperatureReadingDto
        {
            DeviceId = entity.DeviceId,
            CreatedAt = entity.CreatedAt,
            Temperature = entity.Temperature,
            SignalStrength = entity.SignalStrength
        };
    }

    // public async Task<IReadOnlyList<TemperatureReadingDto>> GetReadingsLast24hAsync(Guid deviceId)
    // {
    //     var entities = await _readingRepository.GetReadingsForLast24hWithMinMax(deviceId);
    //     return entities.Select(e => new TemperatureReadingDto
    //     {
    //         DeviceId = e.DeviceId,
    //         Timestamp = e.Timestamp,
    //         Value = e.Value
    //     }).ToList();
    // }

    public async Task<IReadOnlyList<DailyTemperatureStatsDto>> GetDailyStatsLast30DaysAsync(Guid deviceId)
    {
        var stats = await _readingRepository.GetDailyStatsForLast30DaysAsync(deviceId);
        return stats.Select(s => new DailyTemperatureStatsDto
        {
            Date = s.Date,
            Average = s.Average,
            Lowest = s.Minimum,
            Highest = s.Maximum
        }).ToList();
    }
}

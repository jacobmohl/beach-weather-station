using BeachWeatherStation.Application.DTOs;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Domain.Entities;
using BeachWeatherStation.Application.Validators;

namespace BeachWeatherStation.Application.Services;

public class BatteryChangeService
{
    private readonly IBatteryChangeRepository _batteryChangeRepository;
    private readonly BatteryChangeValidator _validator;

    public BatteryChangeService(IBatteryChangeRepository batteryChangeRepository, BatteryChangeValidator validator)
    {
        _batteryChangeRepository = batteryChangeRepository;
        _validator = validator;
    }

    public async Task<bool> IngestBatteryChangeAsync(CreateBatteryChangeDto dto)
    {
        var validationResult = _validator.Validate(dto);
        if (!validationResult.IsValid)
            return false;

        var entity = new BatteryChange
        {
            Id = Guid.NewGuid(),
            DeviceId = dto.DeviceId,
            CreatedAt = dto.CreatedAt
        };
        await _batteryChangeRepository.AddBatteryChangeAsync(entity);
        return true;
    }

    public async Task<BatteryChangeDto?> GetLatestBatteryChangeAsync(Guid deviceId)
    {
        var entity = await _batteryChangeRepository.GetLatestBatteryChangeAsync(deviceId);
        if (entity == null) return null;
        
        return new BatteryChangeDto
        {
            DeviceId = entity.DeviceId,
            CreatedAt = entity.CreatedAt
        };
    }

    public async Task<IReadOnlyList<BatteryChangeDto>> GetBatteryChangesLast24hAsync(Guid deviceId)
    {
        var entities = await _batteryChangeRepository.GetBatteryChangesLast24hAsync(deviceId);
        
        return entities.Select(e => new BatteryChangeDto
        {
            DeviceId = e.DeviceId,
            CreatedAt = e.CreatedAt
        }).ToList();
    }
}

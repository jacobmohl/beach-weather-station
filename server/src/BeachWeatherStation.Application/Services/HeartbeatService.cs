using BeachWeatherStation.Application.DTOs;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Domain.Entities;
using BeachWeatherStation.Application.Validators;

namespace BeachWeatherStation.Application.Services;

public class HeartbeatService
{
    private readonly IHeartbeatRepository _heartbeatRepository;
    private readonly HeartbeatValidator _validator;

    public HeartbeatService(IHeartbeatRepository heartbeatRepository, HeartbeatValidator validator)
    {
        _heartbeatRepository = heartbeatRepository;
        _validator = validator;
    }

    public async Task<bool> IngestHeartbeatAsync(CreateHeartbeatDto dto)
    {
        var validationResult = _validator.Validate(dto);
        if (!validationResult.IsValid)
            return false;

        var entity = new Heartbeat
        {
            Id = Guid.NewGuid(),
            DeviceId = dto.DeviceId,
            CreatedAt = dto.CreatedAt
        };
        await _heartbeatRepository.AddHeartbeatAsync(entity);
        return true;
    }

    public async Task<HeartbeatDto?> GetLatestHeartbeatAsync(Guid deviceId)
    {
        var entity = await _heartbeatRepository.GetLatestHeartbeatAsync(deviceId);
        if (entity == null) return null;
        
        return new HeartbeatDto
        {
            DeviceId = entity.DeviceId,
            CreatedAt = entity.CreatedAt
        };
    }

    public async Task<IReadOnlyList<HeartbeatDto>> GetHeartbeatsLast24hAsync(Guid deviceId)
    {
        var entities = await _heartbeatRepository.GetHeartbeatsLast24hAsync(deviceId);
        
        return entities.Select(e => new HeartbeatDto
        {
            DeviceId = e.DeviceId,
            CreatedAt = e.CreatedAt
        }).ToList();
    }
}

namespace BeachWeatherStation.Application.Validators;

using BeachWeatherStation.Application.DTOs;

public class HeartbeatValidator
{
    public ValidationResult Validate(CreateHeartbeatDto dto)
    {
        if (dto.DeviceId == default)
            return ValidationResult.Invalid("DeviceId is required.");
        if (dto.CreatedAt == default)
            return ValidationResult.Invalid("Timestamp is required.");
        return ValidationResult.Valid();
    }
}

namespace BeachWeatherStation.Application.Validators;

using BeachWeatherStation.Application.DTOs;

public class BatteryChangeValidator
{
    public ValidationResult Validate(CreateBatteryChangeDto dto)
    {
        if (dto.DeviceId == default)
            return ValidationResult.Invalid("DeviceId is required.");
        if (dto.CreatedAt == default)
            return ValidationResult.Invalid("Timestamp is required.");
        return ValidationResult.Valid();
    }
}

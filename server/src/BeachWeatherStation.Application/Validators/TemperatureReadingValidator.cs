namespace BeachWeatherStation.Application.Validators;

using BeachWeatherStation.Application.DTOs;

public class TemperatureReadingValidator
{
    public ValidationResult Validate(CreateTemperatureReadingDto dto)
    {
        if (dto.DeviceId == default)
            return ValidationResult.Invalid("DeviceId is required.");
        if (dto.CreatedAt == default)
            return ValidationResult.Invalid("Timestamp is required.");
        // Add more validation as needed
        return ValidationResult.Valid();
    }
}
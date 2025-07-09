namespace BeachWeatherStation.Application.Validators;

using BeachWeatherStation.Application.DTOs;

public class BatteryChangeValidator
{
    public ValidationResult Validate(CreateBatteryChangeDto dto)
    {
        // No buisiness rules defined yet, so we return valid by default.
        return ValidationResult.Valid();
    }
}

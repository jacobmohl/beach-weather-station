namespace BeachWeatherStation.Application.DTOs;

public class ValidationResult
{
    public bool IsValid { get; }
    public string? Error { get; }

    private ValidationResult(bool isValid, string? error)
    {
        IsValid = isValid;
        Error = error;
    }

    public static ValidationResult Valid() => new ValidationResult(true, null);
    public static ValidationResult Invalid(string error) => new ValidationResult(false, error);
}

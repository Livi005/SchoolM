namespace SchoolManagement.Application.Common;

public interface IValidationService
{
    bool IsValidName(string? name);
    bool IsInRange(double value, double min, double max);
}

public class ValidationService : IValidationService
{
    private const int MaxNameLength = 100;

    public bool IsValidName(string? name)
        => !string.IsNullOrWhiteSpace(name) && name.Length <= MaxNameLength;

    public bool IsInRange(double value, double min, double max)
        => value >= min && value <= max;
}
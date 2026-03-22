namespace ERP.Application.Admissions.Helpers;

public static class AcademicPercentageParser
{
    public static decimal ParseClassXiiPercentage(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0;
        }

        var cleaned = value.Trim().Replace("%", "", StringComparison.Ordinal).Trim();
        if (decimal.TryParse(cleaned, out var result))
        {
            return Math.Clamp(result, 0, 100);
        }

        return 0;
    }
}

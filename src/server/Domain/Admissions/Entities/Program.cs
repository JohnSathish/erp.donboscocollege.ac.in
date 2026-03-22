namespace ERP.Domain.Admissions.Entities;

public class Program
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string Level { get; private set; } = string.Empty; // e.g., "UG", "PG", "Diploma"

    public int DurationYears { get; private set; }

    public int TotalCredits { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private Program()
    {
    }

    public Program(
        string code,
        string name,
        string level,
        int durationYears,
        int totalCredits,
        string? description = null,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Program code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Program name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(level))
            throw new ArgumentException("Program level is required.", nameof(level));
        if (durationYears <= 0)
            throw new ArgumentException("Duration must be greater than 0.", nameof(durationYears));
        if (totalCredits <= 0)
            throw new ArgumentException("Total credits must be greater than 0.", nameof(totalCredits));

        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        Level = level.Trim().ToUpperInvariant();
        DurationYears = durationYears;
        TotalCredits = totalCredits;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string name,
        string level,
        int durationYears,
        int totalCredits,
        string? description = null,
        string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Program name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(level))
            throw new ArgumentException("Program level is required.", nameof(level));
        if (durationYears <= 0)
            throw new ArgumentException("Duration must be greater than 0.", nameof(durationYears));
        if (totalCredits <= 0)
            throw new ArgumentException("Total credits must be greater than 0.", nameof(totalCredits));

        Name = name.Trim();
        Level = level.Trim().ToUpperInvariant();
        DurationYears = durationYears;
        TotalCredits = totalCredits;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void ToggleStatus(string? updatedBy = null)
    {
        IsActive = !IsActive;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}










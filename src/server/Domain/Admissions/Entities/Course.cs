namespace ERP.Domain.Admissions.Entities;

public class Course
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public Guid? ProgramId { get; private set; }

    public Program? Program { get; private set; }

    public int CreditHours { get; private set; }

    public string? Prerequisites { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private Course()
    {
    }

    public Course(
        string code,
        string name,
        int creditHours,
        Guid? programId = null,
        string? description = null,
        string? prerequisites = null,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Course code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Course name is required.", nameof(name));
        if (creditHours <= 0)
            throw new ArgumentException("Credit hours must be greater than 0.", nameof(creditHours));

        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        CreditHours = creditHours;
        ProgramId = programId;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Prerequisites = string.IsNullOrWhiteSpace(prerequisites) ? null : prerequisites.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string name,
        int creditHours,
        Guid? programId = null,
        string? description = null,
        string? prerequisites = null,
        string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Course name is required.", nameof(name));
        if (creditHours <= 0)
            throw new ArgumentException("Credit hours must be greater than 0.", nameof(creditHours));

        Name = name.Trim();
        CreditHours = creditHours;
        ProgramId = programId;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Prerequisites = string.IsNullOrWhiteSpace(prerequisites) ? null : prerequisites.Trim();
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










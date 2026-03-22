namespace ERP.Domain.Admissions.Entities;

public class FeeStructure
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public Guid? ProgramId { get; private set; }

    public Program? Program { get; private set; }

    public string AcademicYear { get; private set; } = string.Empty; // e.g., "2024-2025"

    public bool IsActive { get; private set; } = true;

    public DateTime EffectiveFromUtc { get; private set; }

    public DateTime? EffectiveToUtc { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    public ICollection<FeeComponent> Components { get; private set; } = new List<FeeComponent>();

    private FeeStructure()
    {
    }

    public FeeStructure(
        string name,
        string academicYear,
        DateTime effectiveFromUtc,
        Guid? programId = null,
        string? description = null,
        DateTime? effectiveToUtc = null,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Fee structure name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(academicYear))
            throw new ArgumentException("Academic year is required.", nameof(academicYear));

        Name = name.Trim();
        AcademicYear = academicYear.Trim();
        EffectiveFromUtc = effectiveFromUtc;
        EffectiveToUtc = effectiveToUtc;
        ProgramId = programId;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string name,
        string academicYear,
        DateTime effectiveFromUtc,
        Guid? programId = null,
        string? description = null,
        DateTime? effectiveToUtc = null,
        string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Fee structure name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(academicYear))
            throw new ArgumentException("Academic year is required.", nameof(academicYear));

        Name = name.Trim();
        AcademicYear = academicYear.Trim();
        EffectiveFromUtc = effectiveFromUtc;
        EffectiveToUtc = effectiveToUtc;
        ProgramId = programId;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void AddComponent(FeeComponent component)
    {
        if (component == null)
            throw new ArgumentNullException(nameof(component));

        Components.Add(component);
    }

    public void RemoveComponent(FeeComponent component)
    {
        if (component == null)
            throw new ArgumentNullException(nameof(component));

        Components.Remove(component);
    }

    public decimal GetTotalAmount()
    {
        return Components.Sum(c => c.Amount);
    }

    public void ToggleStatus(string? updatedBy = null)
    {
        IsActive = !IsActive;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}










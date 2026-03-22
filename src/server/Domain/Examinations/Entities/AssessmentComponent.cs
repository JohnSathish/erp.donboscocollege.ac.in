namespace ERP.Domain.Examinations.Entities;

/// <summary>
/// Represents a component/sub-part of an assessment (e.g., Theory, Practical, Viva).
/// </summary>
public class AssessmentComponent
{
    public Guid Id { get; private set; }
    public Guid AssessmentId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Code { get; private set; }
    public int MaxMarks { get; private set; }
    public int PassingMarks { get; private set; }
    public decimal Weightage { get; private set; }
    public int DisplayOrder { get; private set; }
    public string? Instructions { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public Assessment Assessment { get; private set; } = null!;
    public ICollection<MarkEntry> MarkEntries { get; private set; } = new List<MarkEntry>();

    private AssessmentComponent() { } // For EF Core

    public AssessmentComponent(
        Guid assessmentId,
        string name,
        int maxMarks,
        int passingMarks,
        decimal weightage,
        int displayOrder,
        string? code = null,
        string? instructions = null,
        string? createdBy = null)
    {
        Id = Guid.NewGuid();
        AssessmentId = assessmentId;
        Name = name.Trim();
        Code = code?.Trim().ToUpperInvariant();
        MaxMarks = maxMarks;
        PassingMarks = passingMarks;
        Weightage = weightage;
        DisplayOrder = displayOrder;
        Instructions = instructions?.Trim();
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string name,
        int maxMarks,
        int passingMarks,
        decimal weightage,
        int displayOrder,
        string? code = null,
        string? instructions = null,
        string? updatedBy = null)
    {
        Name = name.Trim();
        Code = code?.Trim().ToUpperInvariant();
        MaxMarks = maxMarks;
        PassingMarks = passingMarks;
        Weightage = weightage;
        DisplayOrder = displayOrder;
        Instructions = instructions?.Trim();
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}


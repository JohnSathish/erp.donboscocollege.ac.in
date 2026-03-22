namespace ERP.Domain.Examinations.Entities;

/// <summary>
/// Represents an assessment/examination definition for a course in a specific term.
/// </summary>
public class Assessment
{
    public Guid Id { get; private set; }
    public Guid CourseId { get; private set; }
    public Guid AcademicTermId { get; private set; }
    public Guid? ClassSectionId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public AssessmentType Type { get; private set; }
    public DateTime? ScheduledDate { get; private set; }
    public TimeSpan? Duration { get; private set; }
    public decimal TotalWeightage { get; private set; }
    public int MaxMarks { get; private set; }
    public int PassingMarks { get; private set; }
    public string? Instructions { get; private set; }
    public AssessmentStatus Status { get; private set; }
    public bool IsPublished { get; private set; }
    public DateTime? PublishedOnUtc { get; private set; }
    public string? PublishedBy { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public ICollection<AssessmentComponent> Components { get; private set; } = new List<AssessmentComponent>();
    public ICollection<MarkEntry> MarkEntries { get; private set; } = new List<MarkEntry>();

    private Assessment() { } // For EF Core

    public Assessment(
        Guid courseId,
        Guid academicTermId,
        string name,
        string code,
        AssessmentType type,
        int maxMarks,
        int passingMarks,
        decimal totalWeightage,
        Guid? classSectionId = null,
        DateTime? scheduledDate = null,
        TimeSpan? duration = null,
        string? instructions = null,
        string? createdBy = null)
    {
        Id = Guid.NewGuid();
        CourseId = courseId;
        AcademicTermId = academicTermId;
        ClassSectionId = classSectionId;
        Name = name.Trim();
        Code = code.Trim().ToUpperInvariant();
        Type = type;
        ScheduledDate = scheduledDate;
        Duration = duration;
        TotalWeightage = totalWeightage;
        MaxMarks = maxMarks;
        PassingMarks = passingMarks;
        Instructions = instructions?.Trim();
        Status = AssessmentStatus.Draft;
        IsPublished = false;
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string name,
        string code,
        AssessmentType type,
        int maxMarks,
        int passingMarks,
        decimal totalWeightage,
        DateTime? scheduledDate = null,
        TimeSpan? duration = null,
        string? instructions = null,
        string? updatedBy = null)
    {
        if (Status == AssessmentStatus.Completed)
            throw new InvalidOperationException("Cannot update a completed assessment.");

        Name = name.Trim();
        Code = code.Trim().ToUpperInvariant();
        Type = type;
        MaxMarks = maxMarks;
        PassingMarks = passingMarks;
        TotalWeightage = totalWeightage;
        ScheduledDate = scheduledDate;
        Duration = duration;
        Instructions = instructions?.Trim();
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Publish(string? publishedBy = null)
    {
        if (Status != AssessmentStatus.Draft && Status != AssessmentStatus.Scheduled)
            throw new InvalidOperationException("Only draft or scheduled assessments can be published.");

        IsPublished = true;
        PublishedOnUtc = DateTime.UtcNow;
        PublishedBy = publishedBy;
        Status = AssessmentStatus.Published;
        UpdatedBy = publishedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Unpublish(string? updatedBy = null)
    {
        if (Status == AssessmentStatus.Completed)
            throw new InvalidOperationException("Cannot unpublish a completed assessment.");

        IsPublished = false;
        PublishedOnUtc = null;
        PublishedBy = null;
        Status = AssessmentStatus.Draft;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsCompleted(string? updatedBy = null)
    {
        if (!IsPublished)
            throw new InvalidOperationException("Only published assessments can be marked as completed.");

        Status = AssessmentStatus.Completed;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Cancel(string? reason = null, string? updatedBy = null)
    {
        if (Status == AssessmentStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed assessment.");

        Status = AssessmentStatus.Cancelled;
        Instructions = string.IsNullOrWhiteSpace(reason)
            ? Instructions
            : $"{Instructions}\n\nCancelled: {reason}".Trim();
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum AssessmentType
{
    MidTerm,
    Final,
    Assignment,
    Project,
    Practical,
    Viva,
    Quiz,
    ContinuousAssessment,
    Internal,
    External
}

public enum AssessmentStatus
{
    Draft,
    Scheduled,
    Published,
    InProgress,
    Completed,
    Cancelled
}


namespace ERP.Domain.Examinations.Entities;

/// <summary>
/// Represents marks entered for a student in an assessment component.
/// </summary>
public class MarkEntry
{
    public Guid Id { get; private set; }
    public Guid AssessmentComponentId { get; private set; }
    public Guid StudentId { get; private set; }
    public decimal MarksObtained { get; private set; }
    public decimal? Percentage { get; private set; }
    public string? Grade { get; private set; }
    public string? Remarks { get; private set; }
    public MarkEntryStatus Status { get; private set; }
    public bool IsAbsent { get; private set; }
    public bool IsExempted { get; private set; }
    public DateTime? EnteredOnUtc { get; private set; }
    public string? EnteredBy { get; private set; }
    public DateTime? ApprovedOnUtc { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public string? ModifiedBy { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public AssessmentComponent AssessmentComponent { get; private set; } = null!;

    private MarkEntry() { } // For EF Core

    public MarkEntry(
        Guid assessmentComponentId,
        Guid studentId,
        decimal marksObtained,
        bool isAbsent = false,
        bool isExempted = false,
        string? remarks = null,
        string? enteredBy = null)
    {
        Id = Guid.NewGuid();
        AssessmentComponentId = assessmentComponentId;
        StudentId = studentId;
        MarksObtained = marksObtained;
        IsAbsent = isAbsent;
        IsExempted = isExempted;
        Remarks = remarks?.Trim();
        Status = MarkEntryStatus.Draft;
        EnteredOnUtc = DateTime.UtcNow;
        EnteredBy = enteredBy;
        CreatedBy = enteredBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateMarks(
        decimal marksObtained,
        bool isAbsent = false,
        bool isExempted = false,
        string? remarks = null,
        string? modifiedBy = null)
    {
        if (Status == MarkEntryStatus.Approved && string.IsNullOrWhiteSpace(modifiedBy))
            throw new InvalidOperationException("Cannot modify approved marks without approver information.");

        MarksObtained = marksObtained;
        IsAbsent = isAbsent;
        IsExempted = isExempted;
        Remarks = remarks?.Trim();
        Status = MarkEntryStatus.Draft;
        ModifiedOnUtc = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
        UpdatedBy = modifiedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void CalculatePercentage(int maxMarks)
    {
        if (IsAbsent || IsExempted)
        {
            Percentage = null;
            return;
        }

        if (maxMarks > 0)
        {
            Percentage = (MarksObtained / maxMarks) * 100;
        }
    }

    public void AssignGrade(string grade, string? modifiedBy = null)
    {
        Grade = grade?.Trim();
        UpdatedBy = modifiedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Approve(string? approvedBy = null)
    {
        if (Status == MarkEntryStatus.Approved)
            throw new InvalidOperationException("Marks are already approved.");

        Status = MarkEntryStatus.Approved;
        ApprovedOnUtc = DateTime.UtcNow;
        ApprovedBy = approvedBy;
        UpdatedBy = approvedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Reject(string? reason = null, string? modifiedBy = null)
    {
        Status = MarkEntryStatus.Rejected;
        Remarks = string.IsNullOrWhiteSpace(reason)
            ? Remarks
            : $"{Remarks}\n\nRejected: {reason}".Trim();
        ModifiedOnUtc = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
        UpdatedBy = modifiedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum MarkEntryStatus
{
    Draft,
    Submitted,
    Approved,
    Rejected
}


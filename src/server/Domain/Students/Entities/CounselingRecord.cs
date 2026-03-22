namespace ERP.Domain.Students.Entities;

public class CounselingRecord
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public string SessionType { get; private set; } = string.Empty; // e.g., "Academic", "Personal", "Career", "Behavioral"
    public DateTime SessionDate { get; private set; }
    public string? CounselorName { get; private set; }
    public string? CounselorId { get; private set; }
    public string? Location { get; private set; }
    public string? Issues { get; private set; }
    public string? Discussion { get; private set; }
    public string? Recommendations { get; private set; }
    public string? ActionPlan { get; private set; }
    public bool IsFollowUpRequired { get; private set; } = false;
    public DateTime? FollowUpDate { get; private set; }
    public bool IsConfidential { get; private set; } = false;
    public CounselingStatus Status { get; private set; } = CounselingStatus.Scheduled;
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private CounselingRecord() { }

    public CounselingRecord(
        Guid studentId,
        string sessionType,
        DateTime sessionDate,
        string? counselorName = null,
        string? counselorId = null,
        string? location = null,
        string? createdBy = null)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (string.IsNullOrWhiteSpace(sessionType))
            throw new ArgumentException("Session type is required.", nameof(sessionType));

        StudentId = studentId;
        SessionType = sessionType.Trim();
        SessionDate = sessionDate;
        CounselorName = string.IsNullOrWhiteSpace(counselorName) ? null : counselorName.Trim();
        CounselorId = string.IsNullOrWhiteSpace(counselorId) ? null : counselorId.Trim();
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateSessionDetails(
        string? issues = null,
        string? discussion = null,
        string? recommendations = null,
        string? actionPlan = null,
        string? updatedBy = null)
    {
        Issues = string.IsNullOrWhiteSpace(issues) ? null : issues.Trim();
        Discussion = string.IsNullOrWhiteSpace(discussion) ? null : discussion.Trim();
        Recommendations = string.IsNullOrWhiteSpace(recommendations) ? null : recommendations.Trim();
        ActionPlan = string.IsNullOrWhiteSpace(actionPlan) ? null : actionPlan.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsCompleted(string? updatedBy = null)
    {
        Status = CounselingStatus.Completed;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsCancelled(string? reason = null, string? updatedBy = null)
    {
        Status = CounselingStatus.Cancelled;
        Remarks = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void SetFollowUp(DateTime followUpDate, string? updatedBy = null)
    {
        IsFollowUpRequired = true;
        FollowUpDate = followUpDate;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsConfidential(bool isConfidential, string? updatedBy = null)
    {
        IsConfidential = isConfidential;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateRemarks(string remarks, string? updatedBy = null)
    {
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum CounselingStatus
{
    Scheduled = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3,
    NoShow = 4
}


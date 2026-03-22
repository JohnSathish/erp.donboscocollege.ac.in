namespace ERP.Domain.Students.Entities;

public class StudentExit
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public ExitType ExitType { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public ExitStatus Status { get; private set; } = ExitStatus.Pending;
    public DateTime RequestedDate { get; private set; } = DateTime.UtcNow;
    public DateTime? EffectiveDate { get; private set; }
    public string? RequestedBy { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedOnUtc { get; private set; }
    public string? RejectionReason { get; private set; }
    public bool IsClearanceCompleted { get; private set; } = false;
    public DateTime? ClearanceCompletedOnUtc { get; private set; }
    public string? ClearanceCompletedBy { get; private set; }
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private StudentExit() { }

    public StudentExit(
        Guid studentId,
        ExitType exitType,
        string reason,
        DateTime? effectiveDate = null,
        string? requestedBy = null,
        string? remarks = null)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required.", nameof(reason));

        StudentId = studentId;
        ExitType = exitType;
        Reason = reason.Trim();
        EffectiveDate = effectiveDate ?? DateTime.UtcNow.Date;
        RequestedBy = string.IsNullOrWhiteSpace(requestedBy) ? null : requestedBy.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = requestedBy;
        CreatedOnUtc = DateTime.UtcNow;
        RequestedDate = DateTime.UtcNow;
    }

    public void Approve(string approvedBy, string? remarks = null)
    {
        if (Status != ExitStatus.Pending)
            throw new InvalidOperationException("Only pending exits can be approved.");

        Status = ExitStatus.Approved;
        ApprovedBy = string.IsNullOrWhiteSpace(approvedBy) ? null : approvedBy.Trim();
        ApprovedOnUtc = DateTime.UtcNow;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = approvedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Reject(string rejectedBy, string rejectionReason)
    {
        if (Status != ExitStatus.Pending)
            throw new InvalidOperationException("Only pending exits can be rejected.");

        if (string.IsNullOrWhiteSpace(rejectionReason))
            throw new ArgumentException("Rejection reason is required.", nameof(rejectionReason));

        Status = ExitStatus.Rejected;
        RejectionReason = rejectionReason.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(rejectedBy) ? null : rejectedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void CompleteClearance(string completedBy, string? remarks = null)
    {
        if (Status != ExitStatus.Approved)
            throw new InvalidOperationException("Clearance can only be completed for approved exits.");

        IsClearanceCompleted = true;
        ClearanceCompletedOnUtc = DateTime.UtcNow;
        ClearanceCompletedBy = string.IsNullOrWhiteSpace(completedBy) ? null : completedBy.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = completedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Complete(string completedBy, string? remarks = null)
    {
        if (Status != ExitStatus.Approved || !IsClearanceCompleted)
            throw new InvalidOperationException("Exit can only be completed after clearance is completed.");

        Status = ExitStatus.Completed;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(completedBy) ? null : completedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Cancel(string cancelledBy, string? remarks = null)
    {
        if (Status == ExitStatus.Completed)
            throw new InvalidOperationException("Completed exits cannot be cancelled.");

        Status = ExitStatus.Cancelled;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(cancelledBy) ? null : cancelledBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum ExitType
{
    Withdrawal = 0,
    Dismissal = 1,
    Graduation = 2,
    Transfer = 3,
    Other = 4
}

public enum ExitStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Completed = 3,
    Cancelled = 4
}


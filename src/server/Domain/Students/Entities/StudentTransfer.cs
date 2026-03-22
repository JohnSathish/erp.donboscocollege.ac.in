namespace ERP.Domain.Students.Entities;

public class StudentTransfer
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public Guid? FromProgramId { get; private set; }
    public string? FromProgramCode { get; private set; }
    public Guid? ToProgramId { get; private set; }
    public string? ToProgramCode { get; private set; }
    public string? FromShift { get; private set; }
    public string? ToShift { get; private set; }
    public string? FromSection { get; private set; }
    public string? ToSection { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public TransferStatus Status { get; private set; } = TransferStatus.Pending;
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedOnUtc { get; private set; }
    public string? RejectionReason { get; private set; }
    public DateTime RequestedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? RequestedBy { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private StudentTransfer() { }

    public StudentTransfer(
        Guid studentId,
        Guid? fromProgramId,
        string? fromProgramCode,
        Guid? toProgramId,
        string? toProgramCode,
        string? fromShift,
        string? toShift,
        string? fromSection,
        string? toSection,
        string reason,
        DateTime effectiveDate,
        string? requestedBy = null,
        string? remarks = null)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required.", nameof(reason));

        StudentId = studentId;
        FromProgramId = fromProgramId;
        FromProgramCode = string.IsNullOrWhiteSpace(fromProgramCode) ? null : fromProgramCode.Trim().ToUpperInvariant();
        ToProgramId = toProgramId;
        ToProgramCode = string.IsNullOrWhiteSpace(toProgramCode) ? null : toProgramCode.Trim().ToUpperInvariant();
        FromShift = string.IsNullOrWhiteSpace(fromShift) ? null : fromShift.Trim();
        ToShift = string.IsNullOrWhiteSpace(toShift) ? null : toShift.Trim();
        FromSection = string.IsNullOrWhiteSpace(fromSection) ? null : fromSection.Trim();
        ToSection = string.IsNullOrWhiteSpace(toSection) ? null : toSection.Trim();
        Reason = reason.Trim();
        EffectiveDate = effectiveDate;
        RequestedBy = string.IsNullOrWhiteSpace(requestedBy) ? null : requestedBy.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = requestedBy;
        CreatedOnUtc = DateTime.UtcNow;
        RequestedOnUtc = DateTime.UtcNow;
    }

    public void Approve(string approvedBy, string? remarks = null)
    {
        if (Status != TransferStatus.Pending)
            throw new InvalidOperationException("Only pending transfers can be approved.");

        Status = TransferStatus.Approved;
        ApprovedBy = string.IsNullOrWhiteSpace(approvedBy) ? null : approvedBy.Trim();
        ApprovedOnUtc = DateTime.UtcNow;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = approvedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Reject(string rejectedBy, string rejectionReason)
    {
        if (Status != TransferStatus.Pending)
            throw new InvalidOperationException("Only pending transfers can be rejected.");

        if (string.IsNullOrWhiteSpace(rejectionReason))
            throw new ArgumentException("Rejection reason is required.", nameof(rejectionReason));

        Status = TransferStatus.Rejected;
        RejectionReason = rejectionReason.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(rejectedBy) ? null : rejectedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Complete(string completedBy, string? remarks = null)
    {
        if (Status != TransferStatus.Approved)
            throw new InvalidOperationException("Only approved transfers can be completed.");

        Status = TransferStatus.Completed;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(completedBy) ? null : completedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum TransferStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Completed = 3,
    Cancelled = 4
}


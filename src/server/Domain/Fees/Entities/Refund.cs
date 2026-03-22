namespace ERP.Domain.Fees.Entities;

public class Refund
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public Guid PaymentId { get; private set; }
    public Payment Payment { get; private set; } = null!;
    public Guid? InvoiceId { get; private set; }
    public string RefundNumber { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime RequestedDate { get; private set; } = DateTime.UtcNow;
    public DateTime? ProcessedDate { get; private set; }
    public RefundStatus Status { get; private set; } = RefundStatus.Pending;
    public RefundReason Reason { get; private set; }
    public string? ReasonDetails { get; private set; }
    public string? ProcessedBy { get; private set; }
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private Refund() { }

    public Refund(
        Guid studentId,
        Guid paymentId,
        string refundNumber,
        decimal amount,
        RefundReason reason,
        Guid? invoiceId = null,
        string? reasonDetails = null,
        string? remarks = null,
        string? createdBy = null)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (paymentId == Guid.Empty)
            throw new ArgumentException("Payment ID is required.", nameof(paymentId));
        if (string.IsNullOrWhiteSpace(refundNumber))
            throw new ArgumentException("Refund number is required.", nameof(refundNumber));
        if (amount <= 0)
            throw new ArgumentException("Refund amount must be greater than zero.", nameof(amount));

        StudentId = studentId;
        PaymentId = paymentId;
        RefundNumber = refundNumber.Trim().ToUpperInvariant();
        Amount = amount;
        Reason = reason;
        InvoiceId = invoiceId;
        ReasonDetails = string.IsNullOrWhiteSpace(reasonDetails) ? null : reasonDetails.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
        RequestedDate = DateTime.UtcNow;
    }

    public void Approve(string processedBy, string? remarks = null)
    {
        if (Status != RefundStatus.Pending)
            throw new InvalidOperationException("Only pending refunds can be approved.");

        Status = RefundStatus.Approved;
        ProcessedBy = string.IsNullOrWhiteSpace(processedBy) ? null : processedBy.Trim();
        ProcessedDate = DateTime.UtcNow;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? Remarks : remarks.Trim();
        UpdatedBy = processedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Process(string processedBy, string? remarks = null)
    {
        if (Status != RefundStatus.Approved)
            throw new InvalidOperationException("Only approved refunds can be processed.");

        Status = RefundStatus.Processed;
        ProcessedBy = string.IsNullOrWhiteSpace(processedBy) ? null : processedBy.Trim();
        ProcessedDate = DateTime.UtcNow;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? Remarks : remarks.Trim();
        UpdatedBy = processedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Reject(string rejectedBy, string rejectionReason)
    {
        if (Status != RefundStatus.Pending && Status != RefundStatus.Approved)
            throw new InvalidOperationException("Only pending or approved refunds can be rejected.");

        if (string.IsNullOrWhiteSpace(rejectionReason))
            throw new ArgumentException("Rejection reason is required.", nameof(rejectionReason));

        Status = RefundStatus.Rejected;
        ProcessedBy = string.IsNullOrWhiteSpace(rejectedBy) ? null : rejectedBy.Trim();
        Remarks = rejectionReason.Trim();
        UpdatedBy = rejectedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum RefundStatus
{
    Pending = 0,
    Approved = 1,
    Processed = 2,
    Rejected = 3,
    Cancelled = 4
}

public enum RefundReason
{
    DuplicatePayment = 0,
    Overpayment = 1,
    CancelledEnrollment = 2,
    ScholarshipAdjustment = 3,
    AdministrativeError = 4,
    Other = 5
}


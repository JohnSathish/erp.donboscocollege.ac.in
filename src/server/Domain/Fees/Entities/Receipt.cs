namespace ERP.Domain.Fees.Entities;

public class Receipt
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public Guid PaymentId { get; private set; }
    public Payment Payment { get; private set; } = null!;
    public string ReceiptNumber { get; private set; } = string.Empty;
    public DateTime ReceiptDate { get; private set; } = DateTime.UtcNow;
    public decimal Amount { get; private set; }
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private Receipt() { }

    public Receipt(
        Guid studentId,
        Guid paymentId,
        string receiptNumber,
        decimal amount,
        DateTime receiptDate,
        string? remarks = null,
        string? createdBy = null)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (paymentId == Guid.Empty)
            throw new ArgumentException("Payment ID is required.", nameof(paymentId));
        if (string.IsNullOrWhiteSpace(receiptNumber))
            throw new ArgumentException("Receipt number is required.", nameof(receiptNumber));
        if (amount <= 0)
            throw new ArgumentException("Receipt amount must be greater than zero.", nameof(amount));

        StudentId = studentId;
        PaymentId = paymentId;
        ReceiptNumber = receiptNumber.Trim().ToUpperInvariant();
        Amount = amount;
        ReceiptDate = receiptDate;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateRemarks(string remarks, string? updatedBy = null)
    {
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}


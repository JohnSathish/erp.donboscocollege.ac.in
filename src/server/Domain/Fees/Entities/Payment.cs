namespace ERP.Domain.Fees.Entities;

public class Payment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public Guid InvoiceId { get; private set; }
    public Invoice Invoice { get; private set; } = null!;
    public string PaymentNumber { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; } = DateTime.UtcNow;
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public string? PaymentGateway { get; private set; } // e.g., "Razorpay", "Cash", "Bank Transfer"
    public string? TransactionId { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public string? ChequeNumber { get; private set; }
    public DateTime? ChequeDate { get; private set; }
    public string? BankName { get; private set; }
    public string? Remarks { get; private set; }
    public Guid? ReceiptId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private Payment() { }

    public Payment(
        Guid studentId,
        Guid invoiceId,
        string paymentNumber,
        decimal amount,
        PaymentMethod paymentMethod,
        DateTime paymentDate,
        string? paymentGateway = null,
        string? transactionId = null,
        string? referenceNumber = null,
        string? remarks = null,
        string? createdBy = null)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (invoiceId == Guid.Empty)
            throw new ArgumentException("Invoice ID is required.", nameof(invoiceId));
        if (string.IsNullOrWhiteSpace(paymentNumber))
            throw new ArgumentException("Payment number is required.", nameof(paymentNumber));
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero.", nameof(amount));

        StudentId = studentId;
        InvoiceId = invoiceId;
        PaymentNumber = paymentNumber.Trim().ToUpperInvariant();
        Amount = amount;
        PaymentMethod = paymentMethod;
        PaymentDate = paymentDate;
        PaymentGateway = string.IsNullOrWhiteSpace(paymentGateway) ? null : paymentGateway.Trim();
        TransactionId = string.IsNullOrWhiteSpace(transactionId) ? null : transactionId.Trim();
        ReferenceNumber = string.IsNullOrWhiteSpace(referenceNumber) ? null : referenceNumber.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsCompleted(string? transactionId = null, string? updatedBy = null)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be marked as completed.");

        Status = PaymentStatus.Completed;
        TransactionId = string.IsNullOrWhiteSpace(transactionId) ? TransactionId : transactionId.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsFailed(string? reason = null, string? updatedBy = null)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be marked as failed.");

        Status = PaymentStatus.Failed;
        Remarks = string.IsNullOrWhiteSpace(reason) ? Remarks : reason.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsRefunded(string? updatedBy = null)
    {
        if (Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Only completed payments can be marked as refunded.");

        Status = PaymentStatus.Refunded;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateChequeDetails(string chequeNumber, DateTime chequeDate, string bankName, string? updatedBy = null)
    {
        if (PaymentMethod != PaymentMethod.Cheque)
            throw new InvalidOperationException("Cheque details can only be updated for cheque payments.");

        if (string.IsNullOrWhiteSpace(chequeNumber))
            throw new ArgumentException("Cheque number is required.", nameof(chequeNumber));
        if (string.IsNullOrWhiteSpace(bankName))
            throw new ArgumentException("Bank name is required.", nameof(bankName));

        ChequeNumber = chequeNumber.Trim();
        ChequeDate = chequeDate;
        BankName = bankName.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void LinkReceipt(Guid receiptId, string? updatedBy = null)
    {
        if (receiptId == Guid.Empty)
            throw new ArgumentException("Receipt ID is required.", nameof(receiptId));

        ReceiptId = receiptId;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum PaymentMethod
{
    Cash = 0,
    Cheque = 1,
    BankTransfer = 2,
    OnlineGateway = 3,
    Card = 4,
    Other = 5
}

public enum PaymentStatus
{
    Pending = 0,
    Completed = 1,
    Failed = 2,
    Refunded = 3,
    Cancelled = 4
}


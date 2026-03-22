namespace ERP.Domain.Fees.Entities;

public class Invoice
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public string InvoiceNumber { get; private set; } = string.Empty;
    public Guid? FeeStructureId { get; private set; }
    public string AcademicYear { get; private set; } = string.Empty;
    public string? Term { get; private set; } // e.g., "Fall 2024", "Spring 2025"
    public DateTime IssueDate { get; private set; } = DateTime.UtcNow;
    public DateTime DueDate { get; private set; }
    public decimal SubTotal { get; private set; }
    public decimal DiscountAmount { get; private set; } = 0;
    public decimal ScholarshipAmount { get; private set; } = 0;
    public decimal TaxAmount { get; private set; } = 0;
    public decimal TotalAmount { get; private set; }
    public decimal PaidAmount { get; private set; } = 0;
    public decimal BalanceAmount { get; private set; }
    public InvoiceStatus Status { get; private set; } = InvoiceStatus.Draft;
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }
    public ICollection<InvoiceLine> Lines { get; private set; } = new List<InvoiceLine>();

    private Invoice() { }

    public Invoice(
        Guid studentId,
        string invoiceNumber,
        DateTime dueDate,
        string academicYear,
        Guid? feeStructureId = null,
        string? term = null,
        string? remarks = null,
        string? createdBy = null)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (string.IsNullOrWhiteSpace(invoiceNumber))
            throw new ArgumentException("Invoice number is required.", nameof(invoiceNumber));
        if (string.IsNullOrWhiteSpace(academicYear))
            throw new ArgumentException("Academic year is required.", nameof(academicYear));

        StudentId = studentId;
        InvoiceNumber = invoiceNumber.Trim().ToUpperInvariant();
        DueDate = dueDate;
        AcademicYear = academicYear.Trim();
        FeeStructureId = feeStructureId;
        Term = string.IsNullOrWhiteSpace(term) ? null : term.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
        IssueDate = DateTime.UtcNow;
    }

    public void AddLine(InvoiceLine line)
    {
        if (line == null)
            throw new ArgumentNullException(nameof(line));

        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Cannot add lines to a non-draft invoice.");

        Lines.Add(line);
        RecalculateTotals();
    }

    public void RemoveLine(InvoiceLine line)
    {
        if (line == null)
            throw new ArgumentNullException(nameof(line));

        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Cannot remove lines from a non-draft invoice.");

        Lines.Remove(line);
        RecalculateTotals();
    }

    public void ApplyDiscount(decimal discountAmount, string? updatedBy = null)
    {
        if (discountAmount < 0)
            throw new ArgumentException("Discount amount cannot be negative.", nameof(discountAmount));
        if (discountAmount > SubTotal)
            throw new ArgumentException("Discount amount cannot exceed subtotal.", nameof(discountAmount));

        DiscountAmount = discountAmount;
        RecalculateTotals();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void ApplyScholarship(decimal scholarshipAmount, string? updatedBy = null)
    {
        if (scholarshipAmount < 0)
            throw new ArgumentException("Scholarship amount cannot be negative.", nameof(scholarshipAmount));
        if (scholarshipAmount > (SubTotal - DiscountAmount))
            throw new ArgumentException("Scholarship amount cannot exceed subtotal after discount.", nameof(scholarshipAmount));

        ScholarshipAmount = scholarshipAmount;
        RecalculateTotals();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void ApplyTax(decimal taxAmount, string? updatedBy = null)
    {
        if (taxAmount < 0)
            throw new ArgumentException("Tax amount cannot be negative.", nameof(taxAmount));

        TaxAmount = taxAmount;
        RecalculateTotals();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void RecordPayment(decimal amount, string? updatedBy = null)
    {
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero.", nameof(amount));
        if (amount > BalanceAmount)
            throw new ArgumentException("Payment amount cannot exceed balance amount.", nameof(amount));

        PaidAmount += amount;
        BalanceAmount = TotalAmount - PaidAmount;
        
        if (BalanceAmount <= 0)
        {
            Status = InvoiceStatus.Paid;
        }
        else if (PaidAmount > 0)
        {
            Status = InvoiceStatus.PartiallyPaid;
        }

        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void RecordRefund(decimal amount, string? updatedBy = null)
    {
        if (amount <= 0)
            throw new ArgumentException("Refund amount must be greater than zero.", nameof(amount));
        if (amount > PaidAmount)
            throw new ArgumentException("Refund amount cannot exceed paid amount.", nameof(amount));

        PaidAmount -= amount;
        BalanceAmount = TotalAmount - PaidAmount;

        if (PaidAmount == 0 && BalanceAmount == TotalAmount)
        {
            Status = InvoiceStatus.Draft;
        }
        else if (BalanceAmount > 0)
        {
            Status = InvoiceStatus.PartiallyPaid;
        }

        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Publish(string? updatedBy = null)
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Only draft invoices can be published.");

        if (!Lines.Any())
            throw new InvalidOperationException("Cannot publish an invoice without lines.");

        Status = InvoiceStatus.Issued;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Cancel(string? reason = null, string? updatedBy = null)
    {
        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("Cannot cancel a paid invoice.");

        Status = InvoiceStatus.Cancelled;
        Remarks = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    private void RecalculateTotals()
    {
        SubTotal = Lines.Sum(l => l.Amount);
        TotalAmount = SubTotal - DiscountAmount - ScholarshipAmount + TaxAmount;
        BalanceAmount = TotalAmount - PaidAmount;
    }
}

public enum InvoiceStatus
{
    Draft = 0,
    Issued = 1,
    PartiallyPaid = 2,
    Paid = 3,
    Overdue = 4,
    Cancelled = 5,
    Refunded = 6
}


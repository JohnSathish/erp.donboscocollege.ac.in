namespace ERP.Domain.Library.Entities;

public class BookIssue
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid BookId { get; private set; }

    public Guid? StudentId { get; private set; }

    public Guid? StaffId { get; private set; }

    public string IssuedToType { get; private set; } = string.Empty; // "Student" or "Staff"

    public DateTime IssueDate { get; private set; }

    public DateTime DueDate { get; private set; }

    public DateTime? ReturnDate { get; private set; }

    public IssueStatus Status { get; private set; } = IssueStatus.Issued;

    public decimal? FineAmount { get; private set; }

    public string? Remarks { get; private set; }

    public string? IssuedBy { get; private set; }

    public string? ReturnedBy { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public DateTime? UpdatedOnUtc { get; private set; }

    private BookIssue() { } // For EF Core

    public BookIssue(
        Guid bookId,
        Guid? studentId,
        Guid? staffId,
        string issuedToType,
        DateTime issueDate,
        DateTime dueDate,
        string? issuedBy = null,
        string? remarks = null)
    {
        BookId = bookId;
        StudentId = studentId;
        StaffId = staffId;
        IssuedToType = issuedToType;
        IssueDate = issueDate;
        DueDate = dueDate;
        Status = IssueStatus.Issued;
        IssuedBy = issuedBy;
        Remarks = remarks;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Return(DateTime returnDate, decimal? fineAmount = null, string? returnedBy = null, string? remarks = null)
    {
        ReturnDate = returnDate;
        Status = IssueStatus.Returned;
        FineAmount = fineAmount;
        ReturnedBy = returnedBy;
        if (!string.IsNullOrWhiteSpace(remarks))
        {
            Remarks = remarks;
        }
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsLost(string? remarks = null)
    {
        Status = IssueStatus.Lost;
        if (!string.IsNullOrWhiteSpace(remarks))
        {
            Remarks = remarks;
        }
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsDamaged(string? remarks = null)
    {
        Status = IssueStatus.Damaged;
        if (!string.IsNullOrWhiteSpace(remarks))
        {
            Remarks = remarks;
        }
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateFine(decimal fineAmount)
    {
        FineAmount = fineAmount;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum IssueStatus
{
    Issued,
    Returned,
    Overdue,
    Lost,
    Damaged
}





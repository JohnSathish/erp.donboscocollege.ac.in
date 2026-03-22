namespace ERP.Domain.Academics.Entities;

public class AcademicTerm
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string TermName { get; private set; } = string.Empty; // e.g., "Fall 2024", "Semester 1"
    public string AcademicYear { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; } = false;
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private AcademicTerm() { }

    public AcademicTerm(
        string termName,
        string academicYear,
        DateTime startDate,
        DateTime endDate,
        string? remarks = null,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(termName))
            throw new ArgumentException("Term name is required.", nameof(termName));
        if (string.IsNullOrWhiteSpace(academicYear))
            throw new ArgumentException("Academic year is required.", nameof(academicYear));
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date.");

        TermName = termName.Trim();
        AcademicYear = academicYear.Trim();
        StartDate = startDate.Date;
        EndDate = endDate.Date;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Activate(string? updatedBy = null)
    {
        IsActive = true;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Deactivate(string? updatedBy = null)
    {
        IsActive = false;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateDates(DateTime startDate, DateTime endDate, string? updatedBy = null)
    {
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date.");

        StartDate = startDate.Date;
        EndDate = endDate.Date;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}


namespace ERP.Domain.Students.Entities;

public class AcademicRecord
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid StudentId { get; private set; }

    public Guid? TermId { get; private set; }

    public string AcademicYear { get; private set; } = string.Empty;

    public string Semester { get; private set; } = string.Empty; // e.g., "Semester I", "Semester II"

    public decimal? GPA { get; private set; }

    public decimal? CGPA { get; private set; }

    public string? Grade { get; private set; } // e.g., "A+", "A", "B+", etc.

    public string? ResultStatus { get; private set; } // e.g., "Pass", "Fail", "Supplementary"

    public int TotalCredits { get; private set; }

    public int CreditsEarned { get; private set; }

    public string? Remarks { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private AcademicRecord() { }

    public AcademicRecord(
        Guid studentId,
        string academicYear,
        string semester,
        Guid? termId = null,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(academicYear))
            throw new ArgumentException("Academic year is required.", nameof(academicYear));
        if (string.IsNullOrWhiteSpace(semester))
            throw new ArgumentException("Semester is required.", nameof(semester));

        StudentId = studentId;
        AcademicYear = academicYear.Trim();
        Semester = semester.Trim();
        TermId = termId;
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateResults(
        decimal? gpa,
        decimal? cgpa,
        string? grade,
        string? resultStatus,
        int totalCredits,
        int creditsEarned,
        string? remarks = null,
        string? updatedBy = null)
    {
        GPA = gpa;
        CGPA = cgpa;
        Grade = string.IsNullOrWhiteSpace(grade) ? null : grade.Trim();
        ResultStatus = string.IsNullOrWhiteSpace(resultStatus) ? null : resultStatus.Trim();
        TotalCredits = totalCredits;
        CreditsEarned = creditsEarned;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}


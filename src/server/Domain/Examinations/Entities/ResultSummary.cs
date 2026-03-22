namespace ERP.Domain.Examinations.Entities;

/// <summary>
/// Represents a consolidated result summary for a student in a specific term.
/// </summary>
public class ResultSummary
{
    public Guid Id { get; private set; }
    public Guid StudentId { get; private set; }
    public Guid AcademicTermId { get; private set; }
    public decimal TotalMarks { get; private set; }
    public decimal MaxMarks { get; private set; }
    public decimal Percentage { get; private set; }
    public string? Grade { get; private set; }
    public decimal? GPA { get; private set; }
    public ResultStatus Status { get; private set; }
    public int TotalCredits { get; private set; }
    public int EarnedCredits { get; private set; }
    public DateTime? CalculatedOnUtc { get; private set; }
    public string? CalculatedBy { get; private set; }
    public DateTime? PublishedOnUtc { get; private set; }
    public string? PublishedBy { get; private set; }
    public bool IsPublished { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public ICollection<CourseResult> CourseResults { get; private set; } = new List<CourseResult>();

    private ResultSummary() { } // For EF Core

    public ResultSummary(
        Guid studentId,
        Guid academicTermId,
        string? calculatedBy = null)
    {
        Id = Guid.NewGuid();
        StudentId = studentId;
        AcademicTermId = academicTermId;
        Status = ResultStatus.Pending;
        TotalMarks = 0;
        MaxMarks = 0;
        Percentage = 0;
        TotalCredits = 0;
        EarnedCredits = 0;
        IsPublished = false;
        CalculatedBy = calculatedBy;
        CalculatedOnUtc = DateTime.UtcNow;
        CreatedBy = calculatedBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateResults(
        decimal totalMarks,
        decimal maxMarks,
        decimal percentage,
        string? grade = null,
        decimal? gpa = null,
        int totalCredits = 0,
        int earnedCredits = 0,
        string? calculatedBy = null)
    {
        TotalMarks = totalMarks;
        MaxMarks = maxMarks;
        Percentage = percentage;
        Grade = grade?.Trim();
        GPA = gpa;
        TotalCredits = totalCredits;
        EarnedCredits = earnedCredits;
        Status = ResultStatus.Calculated;
        CalculatedOnUtc = DateTime.UtcNow;
        CalculatedBy = calculatedBy;
        UpdatedBy = calculatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Publish(string? publishedBy = null)
    {
        if (Status != ResultStatus.Calculated)
            throw new InvalidOperationException("Only calculated results can be published.");

        IsPublished = true;
        PublishedOnUtc = DateTime.UtcNow;
        PublishedBy = publishedBy;
        Status = ResultStatus.Published;
        UpdatedBy = publishedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Unpublish(string? updatedBy = null)
    {
        IsPublished = false;
        PublishedOnUtc = null;
        PublishedBy = null;
        Status = ResultStatus.Calculated;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

/// <summary>
/// Represents result for a specific course within a term.
/// </summary>
public class CourseResult
{
    public Guid Id { get; private set; }
    public Guid ResultSummaryId { get; private set; }
    public Guid CourseId { get; private set; }
    public Guid? AssessmentId { get; private set; }
    public decimal TotalMarks { get; private set; }
    public decimal MaxMarks { get; private set; }
    public decimal Percentage { get; private set; }
    public string? Grade { get; private set; }
    public decimal? GradePoints { get; private set; }
    public int Credits { get; private set; }
    public bool IsPassed { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public ResultSummary ResultSummary { get; private set; } = null!;

    private CourseResult() { } // For EF Core

    public CourseResult(
        Guid resultSummaryId,
        Guid courseId,
        decimal totalMarks,
        decimal maxMarks,
        decimal percentage,
        int credits,
        Guid? assessmentId = null,
        string? grade = null,
        decimal? gradePoints = null,
        string? createdBy = null)
    {
        Id = Guid.NewGuid();
        ResultSummaryId = resultSummaryId;
        CourseId = courseId;
        AssessmentId = assessmentId;
        TotalMarks = totalMarks;
        MaxMarks = maxMarks;
        Percentage = percentage;
        Grade = grade?.Trim();
        GradePoints = gradePoints;
        Credits = credits;
        IsPassed = percentage >= 40; // Default passing percentage
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateResults(
        decimal totalMarks,
        decimal maxMarks,
        decimal percentage,
        int credits,
        string? grade = null,
        decimal? gradePoints = null,
        string? updatedBy = null)
    {
        TotalMarks = totalMarks;
        MaxMarks = maxMarks;
        Percentage = percentage;
        Grade = grade?.Trim();
        GradePoints = gradePoints;
        Credits = credits;
        IsPassed = percentage >= 40; // Default passing percentage
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum ResultStatus
{
    Pending,
    Calculated,
    Published,
    Withheld
}


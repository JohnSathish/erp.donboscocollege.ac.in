namespace ERP.Domain.Students.Entities;

public class CourseEnrollment
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid StudentId { get; private set; }

    public Guid CourseId { get; private set; }

    public Guid? TermId { get; private set; }

    public Guid? AcademicRecordId { get; private set; }

    public string EnrollmentType { get; private set; } = string.Empty; // "Regular", "Audit", "Supplementary"

    public DateTime EnrolledOnUtc { get; private set; } = DateTime.UtcNow;

    public string? Grade { get; private set; }

    public decimal? MarksObtained { get; private set; }

    public decimal? MaxMarks { get; private set; }

    public string? ResultStatus { get; private set; } // "Pass", "Fail", "Incomplete"

    public bool IsCompleted { get; private set; }

    public DateTime? CompletedOnUtc { get; private set; }

    public string? Remarks { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private CourseEnrollment() { }

    public CourseEnrollment(
        Guid studentId,
        Guid courseId,
        Guid? termId = null,
        string enrollmentType = "Regular",
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(enrollmentType))
            throw new ArgumentException("Enrollment type is required.", nameof(enrollmentType));

        StudentId = studentId;
        CourseId = courseId;
        TermId = termId;
        EnrollmentType = enrollmentType.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        EnrolledOnUtc = DateTime.UtcNow;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateMarks(
        decimal? marksObtained,
        decimal? maxMarks,
        string? grade,
        string? resultStatus,
        string? remarks = null,
        string? updatedBy = null)
    {
        MarksObtained = marksObtained;
        MaxMarks = maxMarks;
        Grade = string.IsNullOrWhiteSpace(grade) ? null : grade.Trim();
        ResultStatus = string.IsNullOrWhiteSpace(resultStatus) ? null : resultStatus.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void LinkToAcademicRecord(Guid academicRecordId)
    {
        AcademicRecordId = academicRecordId;
    }

    public void MarkAsCompleted(DateTime? completedOnUtc = null, string? updatedBy = null)
    {
        IsCompleted = true;
        CompletedOnUtc = completedOnUtc ?? DateTime.UtcNow;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}


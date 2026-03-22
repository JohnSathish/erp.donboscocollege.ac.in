namespace ERP.Application.Students.Interfaces;

public interface IGradeCalculationService
{
    Task<GradeCalculationResult> CalculateGPAForAcademicRecordAsync(
        Guid academicRecordId,
        CancellationToken cancellationToken = default);

    Task<GradeCalculationResult> CalculateCGPAForStudentAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);

    string? CalculateGradeFromPercentage(decimal percentage);

    decimal? CalculateGPAFromCourseEnrollments(IEnumerable<CourseEnrollmentData> enrollments);
}

public sealed record CourseEnrollmentData(
    Guid EnrollmentId,
    decimal? MarksObtained,
    decimal? MaxMarks,
    int? CreditHours);

public sealed record GradeCalculationResult(
    decimal? GPA,
    decimal? CGPA,
    string? Grade,
    int TotalCredits,
    int CreditsEarned,
    bool Success,
    string? ErrorMessage = null);


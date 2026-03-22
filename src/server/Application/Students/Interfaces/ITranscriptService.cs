namespace ERP.Application.Students.Interfaces;

public interface ITranscriptService
{
    Task<TranscriptDto> GenerateTranscriptAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);

    Task<byte[]> GenerateTranscriptPdfAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);
}

public sealed record TranscriptDto(
    Guid StudentId,
    string StudentNumber,
    string FullName,
    DateOnly DateOfBirth,
    string Gender,
    string? ProgramCode,
    string? MajorSubject,
    string? MinorSubject,
    string AcademicYear,
    DateTime EnrollmentDate,
    IReadOnlyCollection<TranscriptSemesterDto> Semesters,
    decimal? OverallCGPA,
    string? OverallGrade,
    DateTime GeneratedOnUtc);

public sealed record TranscriptSemesterDto(
    string AcademicYear,
    string Semester,
    IReadOnlyCollection<TranscriptCourseDto> Courses,
    decimal? GPA,
    string? Grade,
    int TotalCredits,
    int CreditsEarned,
    string? ResultStatus);

public sealed record TranscriptCourseDto(
    string CourseCode,
    string CourseName,
    decimal? MarksObtained,
    decimal? MaxMarks,
    string? Grade,
    string? ResultStatus,
    int? CreditHours);


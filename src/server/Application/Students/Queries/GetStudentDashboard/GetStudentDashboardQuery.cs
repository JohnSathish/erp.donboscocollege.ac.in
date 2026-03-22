using MediatR;

namespace ERP.Application.Students.Queries.GetStudentDashboard;

public sealed record GetStudentDashboardQuery(Guid StudentId) : IRequest<StudentDashboardDto?>;

public sealed record StudentDashboardDto(
    StudentProfileDto Profile,
    AcademicSummaryDto AcademicSummary,
    IReadOnlyCollection<RecentCourseDto> RecentCourses,
    IReadOnlyCollection<GuardianInfoDto> Guardians,
    IReadOnlyCollection<DashboardNotificationDto> Notifications);

public sealed record StudentProfileDto(
    Guid Id,
    string StudentNumber,
    string FullName,
    string Email,
    string MobileNumber,
    string? PhotoUrl,
    string Shift,
    string AcademicYear,
    string? ProgramCode,
    string? MajorSubject,
    string Status,
    DateTime EnrollmentDate);

public sealed record AcademicSummaryDto(
    decimal? CurrentGPA,
    decimal? OverallCGPA,
    string? CurrentGrade,
    int TotalCredits,
    int CreditsEarned,
    int CoursesEnrolled,
    int CoursesCompleted);

public sealed record RecentCourseDto(
    Guid EnrollmentId,
    Guid CourseId,
    string? CourseName,
    string? Grade,
    decimal? MarksObtained,
    decimal? MaxMarks,
    string? ResultStatus,
    bool IsCompleted);

public sealed record GuardianInfoDto(
    Guid Id,
    string Name,
    string Relationship,
    string ContactNumber,
    string? Email,
    bool IsPrimary);

public sealed record DashboardNotificationDto(
    string Id,
    string Title,
    string Message,
    string Type,
    DateTime CreatedOnUtc,
    bool IsRead);


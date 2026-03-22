using MediatR;

namespace ERP.Application.Students.Queries.GetStudentAcademicHistory;

public sealed record GetStudentAcademicHistoryQuery(Guid StudentId) : IRequest<StudentAcademicHistoryDto?>;

public sealed record StudentAcademicHistoryDto(
    Guid StudentId,
    string StudentNumber,
    string FullName,
    IReadOnlyCollection<AcademicRecordDto> AcademicRecords,
    IReadOnlyCollection<CourseEnrollmentDto> CourseEnrollments);

public sealed record AcademicRecordDto(
    Guid Id,
    string AcademicYear,
    string Semester,
    Guid? TermId,
    decimal? GPA,
    decimal? CGPA,
    string? Grade,
    string? ResultStatus,
    int TotalCredits,
    int CreditsEarned,
    string? Remarks,
    DateTime CreatedOnUtc);

public sealed record CourseEnrollmentDto(
    Guid Id,
    Guid CourseId,
    Guid? TermId,
    Guid? AcademicRecordId,
    string EnrollmentType,
    DateTime EnrolledOnUtc,
    string? Grade,
    decimal? MarksObtained,
    decimal? MaxMarks,
    string? ResultStatus,
    bool IsCompleted,
    DateTime? CompletedOnUtc,
    string? Remarks);


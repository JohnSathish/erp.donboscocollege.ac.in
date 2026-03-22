using MediatR;

namespace ERP.Application.Students.Commands.BulkUpdateCourseEnrollmentMarks;

public sealed record BulkUpdateCourseEnrollmentMarksCommand(
    Guid CourseId,
    Guid? TermId,
    IReadOnlyCollection<StudentMarkEntry> StudentMarks,
    decimal MaxMarks,
    string? UpdatedBy = null) : IRequest<BulkUpdateCourseEnrollmentMarksResult>;

public sealed record StudentMarkEntry(
    Guid StudentId,
    decimal MarksObtained,
    string? Grade = null,
    string? ResultStatus = null,
    string? Remarks = null);

public sealed record BulkUpdateCourseEnrollmentMarksResult(
    int TotalProcessed,
    int SuccessCount,
    int FailureCount,
    IReadOnlyCollection<BulkMarkEntryError> Errors);

public sealed record BulkMarkEntryError(
    Guid StudentId,
    string ErrorMessage);


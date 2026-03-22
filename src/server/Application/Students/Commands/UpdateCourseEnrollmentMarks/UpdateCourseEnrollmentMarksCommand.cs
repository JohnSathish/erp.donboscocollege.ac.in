using MediatR;

namespace ERP.Application.Students.Commands.UpdateCourseEnrollmentMarks;

public sealed record UpdateCourseEnrollmentMarksCommand(
    Guid EnrollmentId,
    decimal? MarksObtained,
    decimal? MaxMarks,
    string? Grade = null,
    string? ResultStatus = null,
    string? Remarks = null,
    string? UpdatedBy = null) : IRequest<UpdateCourseEnrollmentMarksResult>;

public sealed record UpdateCourseEnrollmentMarksResult(
    Guid EnrollmentId,
    Guid StudentId,
    bool Success,
    string? ErrorMessage = null);


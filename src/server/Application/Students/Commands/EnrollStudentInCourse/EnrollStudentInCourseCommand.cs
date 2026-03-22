using MediatR;

namespace ERP.Application.Students.Commands.EnrollStudentInCourse;

public sealed record EnrollStudentInCourseCommand(
    Guid StudentId,
    Guid CourseId,
    Guid? TermId = null,
    string EnrollmentType = "Regular",
    string? CreatedBy = null) : IRequest<EnrollStudentInCourseResult>;

public sealed record EnrollStudentInCourseResult(
    Guid EnrollmentId,
    Guid StudentId,
    Guid CourseId,
    bool Success,
    string? ErrorMessage = null);


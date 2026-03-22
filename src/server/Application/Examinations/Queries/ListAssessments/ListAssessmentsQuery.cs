using MediatR;

namespace ERP.Application.Examinations.Queries.ListAssessments;

public record ListAssessmentsQuery(
    Guid? CourseId = null,
    Guid? AcademicTermId = null,
    Guid? ClassSectionId = null,
    string? Status = null) : IRequest<IReadOnlyCollection<AssessmentSummaryDto>>;

public record AssessmentSummaryDto(
    Guid Id,
    Guid CourseId,
    string CourseName,
    Guid AcademicTermId,
    string AcademicTermName,
    string Name,
    string Code,
    string Type,
    DateTime? ScheduledDate,
    int MaxMarks,
    string Status,
    bool IsPublished);






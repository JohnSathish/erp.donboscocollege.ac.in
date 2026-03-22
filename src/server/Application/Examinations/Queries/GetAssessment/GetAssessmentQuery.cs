using MediatR;

namespace ERP.Application.Examinations.Queries.GetAssessment;

public record GetAssessmentQuery(Guid AssessmentId) : IRequest<AssessmentDto?>;

public record AssessmentDto(
    Guid Id,
    Guid CourseId,
    string CourseName,
    Guid AcademicTermId,
    string AcademicTermName,
    Guid? ClassSectionId,
    string? ClassSectionName,
    string Name,
    string Code,
    string Type,
    DateTime? ScheduledDate,
    TimeSpan? Duration,
    decimal TotalWeightage,
    int MaxMarks,
    int PassingMarks,
    string? Instructions,
    string Status,
    bool IsPublished,
    DateTime? PublishedOnUtc,
    string? PublishedBy,
    IReadOnlyCollection<AssessmentComponentDto> Components);

public record AssessmentComponentDto(
    Guid Id,
    string Name,
    string? Code,
    int MaxMarks,
    int PassingMarks,
    decimal Weightage,
    int DisplayOrder,
    string? Instructions);






using MediatR;

namespace ERP.Application.Examinations.Queries.GetResults;

public record GetResultsQuery(
    Guid StudentId,
    Guid AcademicTermId) : IRequest<ResultSummaryDto?>;

public record ResultSummaryDto(
    Guid Id,
    Guid StudentId,
    string StudentNumber,
    string StudentName,
    Guid AcademicTermId,
    string AcademicTermName,
    decimal TotalMarks,
    decimal MaxMarks,
    decimal Percentage,
    string? Grade,
    decimal? GPA,
    int TotalCredits,
    int EarnedCredits,
    string Status,
    bool IsPublished,
    DateTime? PublishedOnUtc,
    string? PublishedBy,
    IReadOnlyCollection<CourseResultDto> CourseResults);

public record CourseResultDto(
    Guid Id,
    Guid CourseId,
    string CourseName,
    Guid? AssessmentId,
    decimal TotalMarks,
    decimal MaxMarks,
    decimal Percentage,
    string? Grade,
    decimal? GradePoints,
    int Credits,
    bool IsPassed);






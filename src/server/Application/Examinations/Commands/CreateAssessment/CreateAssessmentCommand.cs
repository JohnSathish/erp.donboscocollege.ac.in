using MediatR;

namespace ERP.Application.Examinations.Commands.CreateAssessment;

public record CreateAssessmentCommand(
    Guid CourseId,
    Guid AcademicTermId,
    string Name,
    string Code,
    string Type,
    int MaxMarks,
    int PassingMarks,
    decimal TotalWeightage,
    Guid? ClassSectionId = null,
    DateTime? ScheduledDate = null,
    TimeSpan? Duration = null,
    string? Instructions = null,
    List<AssessmentComponentDto>? Components = null,
    string? CreatedBy = null) : IRequest<Guid>;

public record AssessmentComponentDto(
    string Name,
    int MaxMarks,
    int PassingMarks,
    decimal Weightage,
    int DisplayOrder,
    string? Code = null,
    string? Instructions = null);






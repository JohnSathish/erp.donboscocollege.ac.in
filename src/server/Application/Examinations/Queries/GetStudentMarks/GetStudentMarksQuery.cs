using MediatR;

namespace ERP.Application.Examinations.Queries.GetStudentMarks;

public record GetStudentMarksQuery(
    Guid StudentId,
    Guid? AcademicTermId = null) : IRequest<IReadOnlyCollection<MarkEntryDto>>;

public record MarkEntryDto(
    Guid Id,
    Guid AssessmentComponentId,
    string ComponentName,
    Guid AssessmentId,
    string AssessmentName,
    string AssessmentCode,
    decimal MarksObtained,
    decimal? Percentage,
    string? Grade,
    string? Remarks,
    string Status,
    bool IsAbsent,
    bool IsExempted,
    DateTime? EnteredOnUtc,
    string? EnteredBy,
    DateTime? ApprovedOnUtc,
    string? ApprovedBy);






using MediatR;

namespace ERP.Application.Examinations.Commands.EnterMarks;

public record EnterMarksCommand(
    Guid AssessmentComponentId,
    Guid StudentId,
    decimal MarksObtained,
    bool IsAbsent = false,
    bool IsExempted = false,
    string? Remarks = null,
    string? EnteredBy = null) : IRequest<Guid>;






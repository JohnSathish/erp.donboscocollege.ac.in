using MediatR;

namespace ERP.Application.Examinations.Commands.BulkEnterMarks;

public record BulkEnterMarksCommand(
    Guid AssessmentComponentId,
    List<StudentMarkDto> StudentMarks,
    string? EnteredBy = null) : IRequest<int>;

public record StudentMarkDto(
    Guid StudentId,
    decimal MarksObtained,
    bool IsAbsent = false,
    bool IsExempted = false,
    string? Remarks = null);






using MediatR;

namespace ERP.Application.Students.Commands.CreateAcademicRecord;

public sealed record CreateAcademicRecordCommand(
    Guid StudentId,
    string AcademicYear,
    string Semester,
    Guid? TermId = null,
    string? CreatedBy = null) : IRequest<CreateAcademicRecordResult>;

public sealed record CreateAcademicRecordResult(
    Guid RecordId,
    Guid StudentId,
    string AcademicYear,
    string Semester,
    bool Success,
    string? ErrorMessage = null);


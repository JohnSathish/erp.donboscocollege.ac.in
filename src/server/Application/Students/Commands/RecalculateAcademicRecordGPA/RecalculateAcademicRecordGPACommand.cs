using MediatR;

namespace ERP.Application.Students.Commands.RecalculateAcademicRecordGPA;

public sealed record RecalculateAcademicRecordGPACommand(
    Guid AcademicRecordId,
    string? UpdatedBy = null) : IRequest<RecalculateAcademicRecordGPAResult>;

public sealed record RecalculateAcademicRecordGPAResult(
    Guid AcademicRecordId,
    decimal? GPA,
    string? Grade,
    bool Success,
    string? ErrorMessage = null);


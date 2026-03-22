using MediatR;

namespace ERP.Application.Students.Commands.UpdateStudentStatus;

public sealed record UpdateStudentStatusCommand(
    Guid StudentId,
    string Status,
    string? UpdatedBy = null) : IRequest<UpdateStudentStatusResult>;

public sealed record UpdateStudentStatusResult(
    Guid StudentId,
    string StudentNumber,
    string Status,
    bool Success,
    string? ErrorMessage = null);


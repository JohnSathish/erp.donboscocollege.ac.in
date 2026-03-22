using MediatR;

namespace ERP.Application.Students.Commands.ApproveStudentExit;

public sealed record ApproveStudentExitCommand(
    Guid ExitId,
    string? Remarks = null,
    string? ApprovedBy = null) : IRequest<bool>;


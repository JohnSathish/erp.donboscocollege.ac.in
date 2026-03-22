using MediatR;

namespace ERP.Application.Students.Commands.ApproveStudentTransfer;

public sealed record ApproveStudentTransferCommand(
    Guid TransferId,
    string? Remarks = null,
    string? ApprovedBy = null) : IRequest<bool>;


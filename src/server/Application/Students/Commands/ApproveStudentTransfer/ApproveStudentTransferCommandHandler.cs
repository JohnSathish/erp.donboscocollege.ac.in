using ERP.Application.Students.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.ApproveStudentTransfer;

public sealed class ApproveStudentTransferCommandHandler : IRequestHandler<ApproveStudentTransferCommand, bool>
{
    private readonly IStudentTransferRepository _transferRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<ApproveStudentTransferCommandHandler> _logger;

    public ApproveStudentTransferCommandHandler(
        IStudentTransferRepository transferRepository,
        IStudentRepository studentRepository,
        ILogger<ApproveStudentTransferCommandHandler> logger)
    {
        _transferRepository = transferRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(ApproveStudentTransferCommand request, CancellationToken cancellationToken)
    {
        var transfer = await _transferRepository.GetByIdAsync(request.TransferId, cancellationToken);
        if (transfer == null)
        {
            _logger.LogWarning("Transfer with ID {TransferId} not found for approval.", request.TransferId);
            return false;
        }

        transfer.Approve(request.ApprovedBy ?? "System", request.Remarks);
        await _transferRepository.UpdateAsync(transfer, cancellationToken);

        // Update student record if transfer is approved
        var student = await _studentRepository.GetByIdAsync(transfer.StudentId, cancellationToken);
        if (student != null)
        {
            student.Transfer(
                transfer.ToProgramId,
                transfer.ToProgramCode,
                transfer.ToShift,
                transfer.ToSection,
                request.ApprovedBy);

            if (transfer.ToProgramId.HasValue || !string.IsNullOrWhiteSpace(transfer.ToProgramCode))
            {
                student.UpdateStatus(Domain.Students.Entities.StudentStatus.Transferred, request.ApprovedBy);
            }

            await _studentRepository.UpdateAsync(student, cancellationToken);
        }

        _logger.LogInformation("Transfer {TransferId} approved for student {StudentId}.", transfer.Id, transfer.StudentId);
        return true;
    }
}


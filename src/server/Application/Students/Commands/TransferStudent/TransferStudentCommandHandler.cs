using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.TransferStudent;

public sealed class TransferStudentCommandHandler : IRequestHandler<TransferStudentCommand, TransferStudentResult>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentTransferRepository _transferRepository;
    private readonly ILogger<TransferStudentCommandHandler> _logger;

    public TransferStudentCommandHandler(
        IStudentRepository studentRepository,
        IStudentTransferRepository transferRepository,
        ILogger<TransferStudentCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _transferRepository = transferRepository;
        _logger = logger;
    }

    public async Task<TransferStudentResult> Handle(TransferStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            _logger.LogWarning("Student with ID {StudentId} not found for transfer.", request.StudentId);
            return new TransferStudentResult(
                Guid.Empty,
                request.StudentId,
                string.Empty,
                false,
                "Student not found.");
        }

        // Check if there's already a pending transfer
        var pendingTransfer = await _transferRepository.GetByStudentIdAsync(request.StudentId, cancellationToken);
        if (pendingTransfer.Any(t => t.Status == TransferStatus.Pending))
        {
            _logger.LogWarning("Student {StudentId} already has a pending transfer request.", request.StudentId);
            return new TransferStudentResult(
                Guid.Empty,
                request.StudentId,
                student.StudentNumber,
                false,
                "Student already has a pending transfer request.");
        }

        var transfer = new StudentTransfer(
            request.StudentId,
            student.ProgramId,
            student.ProgramCode,
            request.ToProgramId,
            request.ToProgramCode,
            student.Shift,
            request.ToShift,
            null, // FromSection - not tracked in Student entity currently
            request.ToSection,
            request.Reason,
            request.EffectiveDate,
            request.RequestedBy,
            request.Remarks);

        var createdTransfer = await _transferRepository.AddAsync(transfer, cancellationToken);

        _logger.LogInformation("Transfer request {TransferId} created for student {StudentNumber} (ID: {StudentId}).",
            createdTransfer.Id, student.StudentNumber, student.Id);

        return new TransferStudentResult(
            createdTransfer.Id,
            request.StudentId,
            student.StudentNumber,
            true);
    }
}


using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.ApproveStudentExit;

public sealed class ApproveStudentExitCommandHandler : IRequestHandler<ApproveStudentExitCommand, bool>
{
    private readonly IStudentExitRepository _exitRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<ApproveStudentExitCommandHandler> _logger;

    public ApproveStudentExitCommandHandler(
        IStudentExitRepository exitRepository,
        IStudentRepository studentRepository,
        ILogger<ApproveStudentExitCommandHandler> logger)
    {
        _exitRepository = exitRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(ApproveStudentExitCommand request, CancellationToken cancellationToken)
    {
        var exit = await _exitRepository.GetByIdAsync(request.ExitId, cancellationToken);
        if (exit == null)
        {
            _logger.LogWarning("Exit with ID {ExitId} not found for approval.", request.ExitId);
            return false;
        }

        exit.Approve(request.ApprovedBy ?? "System", request.Remarks);
        await _exitRepository.UpdateAsync(exit, cancellationToken);

        // Update student status
        var student = await _studentRepository.GetByIdAsync(exit.StudentId, cancellationToken);
        if (student != null)
        {
            student.UpdateStatus(StudentStatus.Withdrawn, request.ApprovedBy);
            await _studentRepository.UpdateAsync(student, cancellationToken);
        }

        _logger.LogInformation("Exit {ExitId} approved for student {StudentId}.", exit.Id, exit.StudentId);
        return true;
    }
}


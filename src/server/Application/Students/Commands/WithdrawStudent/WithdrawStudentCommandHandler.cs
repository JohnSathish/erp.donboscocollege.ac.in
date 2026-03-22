using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.WithdrawStudent;

public sealed class WithdrawStudentCommandHandler : IRequestHandler<WithdrawStudentCommand, WithdrawStudentResult>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentExitRepository _exitRepository;
    private readonly ILogger<WithdrawStudentCommandHandler> _logger;

    public WithdrawStudentCommandHandler(
        IStudentRepository studentRepository,
        IStudentExitRepository exitRepository,
        ILogger<WithdrawStudentCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _exitRepository = exitRepository;
        _logger = logger;
    }

    public async Task<WithdrawStudentResult> Handle(WithdrawStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            _logger.LogWarning("Student with ID {StudentId} not found for withdrawal.", request.StudentId);
            return new WithdrawStudentResult(
                Guid.Empty,
                request.StudentId,
                string.Empty,
                false,
                "Student not found.");
        }

        // Check if there's already an active exit request
        var activeExit = await _exitRepository.GetActiveExitByStudentIdAsync(request.StudentId, cancellationToken);
        if (activeExit != null)
        {
            _logger.LogWarning("Student {StudentId} already has an active exit request (ID: {ExitId}).", request.StudentId, activeExit.Id);
            return new WithdrawStudentResult(
                activeExit.Id,
                request.StudentId,
                student.StudentNumber,
                false,
                "Student already has an active exit request.");
        }

        var exit = new StudentExit(
            request.StudentId,
            request.ExitType,
            request.Reason,
            request.EffectiveDate,
            request.RequestedBy,
            request.Remarks);

        var createdExit = await _exitRepository.AddAsync(exit, cancellationToken);

        _logger.LogInformation("Exit request {ExitId} created for student {StudentNumber} (ID: {StudentId}), Type: {ExitType}.",
            createdExit.Id, student.StudentNumber, student.Id, request.ExitType);

        return new WithdrawStudentResult(
            createdExit.Id,
            request.StudentId,
            student.StudentNumber,
            true);
    }
}


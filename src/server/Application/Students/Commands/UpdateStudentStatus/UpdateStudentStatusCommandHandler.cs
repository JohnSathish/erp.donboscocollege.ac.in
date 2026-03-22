using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.UpdateStudentStatus;

public sealed class UpdateStudentStatusCommandHandler : IRequestHandler<UpdateStudentStatusCommand, UpdateStudentStatusResult>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<UpdateStudentStatusCommandHandler> _logger;

    public UpdateStudentStatusCommandHandler(
        IStudentRepository studentRepository,
        ILogger<UpdateStudentStatusCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<UpdateStudentStatusResult> Handle(UpdateStudentStatusCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            return new UpdateStudentStatusResult(
                request.StudentId,
                string.Empty,
                request.Status,
                false,
                "Student not found.");
        }

        try
        {
            if (!Enum.TryParse<StudentStatus>(request.Status, ignoreCase: true, out var status))
            {
                return new UpdateStudentStatusResult(
                    request.StudentId,
                    student.StudentNumber,
                    request.Status,
                    false,
                    $"Invalid status: {request.Status}");
            }

            student.UpdateStatus(status, request.UpdatedBy);
            await _studentRepository.UpdateAsync(student, cancellationToken);

            _logger.LogInformation(
                "Updated student status for StudentId: {StudentId}, StudentNumber: {StudentNumber}, NewStatus: {Status}",
                student.Id,
                student.StudentNumber,
                status);

            return new UpdateStudentStatusResult(
                student.Id,
                student.StudentNumber,
                status.ToString(),
                true);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to update student status for StudentId: {StudentId}",
                request.StudentId);

            return new UpdateStudentStatusResult(
                request.StudentId,
                student.StudentNumber,
                request.Status,
                false,
                ex.Message);
        }
    }
}


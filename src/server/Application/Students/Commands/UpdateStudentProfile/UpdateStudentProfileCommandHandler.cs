using ERP.Application.Students.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.UpdateStudentProfile;

public sealed class UpdateStudentProfileCommandHandler : IRequestHandler<UpdateStudentProfileCommand, UpdateStudentProfileResult>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<UpdateStudentProfileCommandHandler> _logger;

    public UpdateStudentProfileCommandHandler(
        IStudentRepository studentRepository,
        ILogger<UpdateStudentProfileCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<UpdateStudentProfileResult> Handle(UpdateStudentProfileCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            return new UpdateStudentProfileResult(
                request.StudentId,
                string.Empty,
                false,
                "Student not found.");
        }

        try
        {
            student.Update(
                request.FullName,
                request.Email,
                request.MobileNumber,
                request.Shift,
                request.ProgramId,
                request.ProgramCode,
                request.MajorSubject,
                request.MinorSubject,
                request.PhotoUrl,
                request.UpdatedBy);

            await _studentRepository.UpdateAsync(student, cancellationToken);

            _logger.LogInformation(
                "Updated student profile for StudentId: {StudentId}, StudentNumber: {StudentNumber}",
                student.Id,
                student.StudentNumber);

            return new UpdateStudentProfileResult(
                student.Id,
                student.StudentNumber,
                true);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to update student profile for StudentId: {StudentId}",
                request.StudentId);

            return new UpdateStudentProfileResult(
                request.StudentId,
                student.StudentNumber,
                false,
                ex.Message);
        }
    }
}


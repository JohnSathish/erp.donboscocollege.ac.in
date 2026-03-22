using ERP.Application.Students.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.ChangeStudentShift;

public sealed class ChangeStudentShiftCommandHandler : IRequestHandler<ChangeStudentShiftCommand, ChangeStudentShiftResult>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<ChangeStudentShiftCommandHandler> _logger;

    public ChangeStudentShiftCommandHandler(
        IStudentRepository studentRepository,
        ILogger<ChangeStudentShiftCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<ChangeStudentShiftResult> Handle(ChangeStudentShiftCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            _logger.LogWarning("Student with ID {StudentId} not found for shift change.", request.StudentId);
            return new ChangeStudentShiftResult(
                request.StudentId,
                string.Empty,
                string.Empty,
                request.NewShift,
                false,
                "Student not found.");
        }

        var oldShift = student.Shift;

        // Validate shift - assuming three shifts: "Morning", "Afternoon", "Evening"
        var validShifts = new[] { "Morning", "Afternoon", "Evening" };
        if (!validShifts.Contains(request.NewShift, StringComparer.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Invalid shift '{NewShift}' provided for student {StudentId}. Valid shifts are: {ValidShifts}",
                request.NewShift, request.StudentId, string.Join(", ", validShifts));
            return new ChangeStudentShiftResult(
                request.StudentId,
                student.StudentNumber,
                oldShift,
                request.NewShift,
                false,
                $"Invalid shift. Valid shifts are: {string.Join(", ", validShifts)}");
        }

        if (string.Equals(oldShift, request.NewShift, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Student {StudentId} is already in shift {Shift}. No change needed.", request.StudentId, request.NewShift);
            return new ChangeStudentShiftResult(
                request.StudentId,
                student.StudentNumber,
                oldShift,
                request.NewShift,
                true);
        }

        student.ChangeShift(request.NewShift, request.UpdatedBy);
        await _studentRepository.UpdateAsync(student, cancellationToken);

        _logger.LogInformation("Student {StudentNumber} (ID: {StudentId}) shift changed from {OldShift} to {NewShift}.",
            student.StudentNumber, student.Id, oldShift, request.NewShift);

        return new ChangeStudentShiftResult(
            request.StudentId,
            student.StudentNumber,
            oldShift,
            request.NewShift,
            true);
    }
}


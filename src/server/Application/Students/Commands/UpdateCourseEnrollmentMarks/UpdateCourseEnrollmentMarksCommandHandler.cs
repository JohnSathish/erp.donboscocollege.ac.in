using ERP.Application.Students.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.UpdateCourseEnrollmentMarks;

public sealed class UpdateCourseEnrollmentMarksCommandHandler : IRequestHandler<UpdateCourseEnrollmentMarksCommand, UpdateCourseEnrollmentMarksResult>
{
    private readonly IAcademicHistoryRepository _academicHistoryRepository;
    private readonly IGradeCalculationService _gradeCalculationService;
    private readonly ILogger<UpdateCourseEnrollmentMarksCommandHandler> _logger;

    public UpdateCourseEnrollmentMarksCommandHandler(
        IAcademicHistoryRepository academicHistoryRepository,
        IGradeCalculationService gradeCalculationService,
        ILogger<UpdateCourseEnrollmentMarksCommandHandler> logger)
    {
        _academicHistoryRepository = academicHistoryRepository;
        _gradeCalculationService = gradeCalculationService;
        _logger = logger;
    }

    public async Task<UpdateCourseEnrollmentMarksResult> Handle(UpdateCourseEnrollmentMarksCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _academicHistoryRepository.GetCourseEnrollmentByIdAsync(request.EnrollmentId, cancellationToken);
        if (enrollment == null)
        {
            return new UpdateCourseEnrollmentMarksResult(
                request.EnrollmentId,
                Guid.Empty,
                false,
                "Course enrollment not found.");
        }

        try
        {
            // Calculate grade if not provided
            string? grade = request.Grade;
            if (string.IsNullOrWhiteSpace(grade) && request.MarksObtained.HasValue && request.MaxMarks.HasValue && request.MaxMarks.Value > 0)
            {
                var percentage = (request.MarksObtained.Value / request.MaxMarks.Value) * 100;
                grade = _gradeCalculationService.CalculateGradeFromPercentage(percentage);
            }

            // Determine result status if not provided
            string? resultStatus = request.ResultStatus;
            if (string.IsNullOrWhiteSpace(resultStatus) && request.MarksObtained.HasValue && request.MaxMarks.HasValue && request.MaxMarks.Value > 0)
            {
                var percentage = (request.MarksObtained.Value / request.MaxMarks.Value) * 100;
                resultStatus = percentage >= 40 ? "Pass" : "Fail";
            }

            enrollment.UpdateMarks(
                request.MarksObtained,
                request.MaxMarks,
                grade,
                resultStatus,
                request.Remarks,
                request.UpdatedBy);

            // Mark as completed if marks are entered
            if (request.MarksObtained.HasValue && request.MaxMarks.HasValue)
            {
                enrollment.MarkAsCompleted(DateTime.UtcNow, request.UpdatedBy);
            }

            await _academicHistoryRepository.UpdateAsync(enrollment, cancellationToken);

            // Recalculate GPA for the academic record if linked
            if (enrollment.AcademicRecordId.HasValue)
            {
                try
                {
                    var gpaResult = await _gradeCalculationService.CalculateGPAForAcademicRecordAsync(
                        enrollment.AcademicRecordId.Value,
                        cancellationToken);

                    if (gpaResult.Success)
                    {
                        var academicRecord = await _academicHistoryRepository.GetAcademicRecordByIdAsync(
                            enrollment.AcademicRecordId.Value,
                            cancellationToken);

                        if (academicRecord != null)
                        {
                            academicRecord.UpdateResults(
                                gpaResult.GPA,
                                null, // CGPA calculated separately
                                gpaResult.Grade,
                                gpaResult.CreditsEarned >= gpaResult.TotalCredits * 0.4m ? "Pass" : "Fail",
                                gpaResult.TotalCredits,
                                gpaResult.CreditsEarned,
                                null,
                                request.UpdatedBy);

                            await _academicHistoryRepository.UpdateAsync(academicRecord, cancellationToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to recalculate GPA for academic record {AcademicRecordId}", enrollment.AcademicRecordId);
                    // Don't fail the mark update if GPA calculation fails
                }
            }

            _logger.LogInformation(
                "Updated marks for enrollment {EnrollmentId}, StudentId: {StudentId}, Marks: {MarksObtained}/{MaxMarks}",
                enrollment.Id,
                enrollment.StudentId,
                request.MarksObtained,
                request.MaxMarks);

            return new UpdateCourseEnrollmentMarksResult(
                enrollment.Id,
                enrollment.StudentId,
                true);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to update marks for enrollment {EnrollmentId}",
                request.EnrollmentId);

            return new UpdateCourseEnrollmentMarksResult(
                request.EnrollmentId,
                enrollment.StudentId,
                false,
                ex.Message);
        }
    }
}


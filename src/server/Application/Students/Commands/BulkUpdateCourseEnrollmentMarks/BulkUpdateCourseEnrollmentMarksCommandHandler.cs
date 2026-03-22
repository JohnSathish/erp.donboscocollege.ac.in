using ERP.Application.Students.Interfaces;
using ERP.Application.Admissions.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.BulkUpdateCourseEnrollmentMarks;

public sealed class BulkUpdateCourseEnrollmentMarksCommandHandler : IRequestHandler<BulkUpdateCourseEnrollmentMarksCommand, BulkUpdateCourseEnrollmentMarksResult>
{
    private readonly IAcademicHistoryRepository _academicHistoryRepository;
    private readonly IGradeCalculationService _gradeCalculationService;
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<BulkUpdateCourseEnrollmentMarksCommandHandler> _logger;

    public BulkUpdateCourseEnrollmentMarksCommandHandler(
        IAcademicHistoryRepository academicHistoryRepository,
        IGradeCalculationService gradeCalculationService,
        ICourseRepository courseRepository,
        ILogger<BulkUpdateCourseEnrollmentMarksCommandHandler> logger)
    {
        _academicHistoryRepository = academicHistoryRepository;
        _gradeCalculationService = gradeCalculationService;
        _courseRepository = courseRepository;
        _logger = logger;
    }

    public async Task<BulkUpdateCourseEnrollmentMarksResult> Handle(BulkUpdateCourseEnrollmentMarksCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course == null)
        {
            return new BulkUpdateCourseEnrollmentMarksResult(
                0,
                0,
                0,
                new[] { new BulkMarkEntryError(Guid.Empty, "Course not found.") });
        }

        var errors = new List<BulkMarkEntryError>();
        var successCount = 0;

        foreach (var studentMark in request.StudentMarks)
        {
            try
            {
                var enrollments = await _academicHistoryRepository.GetCourseEnrollmentsByStudentIdAsync(
                    studentMark.StudentId,
                    request.TermId,
                    cancellationToken);

                var enrollment = enrollments.FirstOrDefault(e => e.CourseId == request.CourseId && !e.IsCompleted);
                if (enrollment == null)
                {
                    errors.Add(new BulkMarkEntryError(
                        studentMark.StudentId,
                        "No active enrollment found for this course and term."));
                    continue;
                }

                // Calculate grade if not provided
                string? grade = studentMark.Grade;
                if (string.IsNullOrWhiteSpace(grade) && request.MaxMarks > 0)
                {
                    var percentage = (studentMark.MarksObtained / request.MaxMarks) * 100;
                    grade = _gradeCalculationService.CalculateGradeFromPercentage(percentage);
                }

                // Determine result status if not provided
                string? resultStatus = studentMark.ResultStatus;
                if (string.IsNullOrWhiteSpace(resultStatus) && request.MaxMarks > 0)
                {
                    var percentage = (studentMark.MarksObtained / request.MaxMarks) * 100;
                    resultStatus = percentage >= 40 ? "Pass" : "Fail";
                }

                enrollment.UpdateMarks(
                    studentMark.MarksObtained,
                    request.MaxMarks,
                    grade,
                    resultStatus,
                    studentMark.Remarks,
                    request.UpdatedBy);

                enrollment.MarkAsCompleted(DateTime.UtcNow, request.UpdatedBy);

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
                                    null,
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

                successCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update marks for student {StudentId} in course {CourseId}", studentMark.StudentId, request.CourseId);
                errors.Add(new BulkMarkEntryError(
                    studentMark.StudentId,
                    ex.Message));
            }
        }

        _logger.LogInformation(
            "Bulk mark entry completed for course {CourseId}. Processed: {Total}, Success: {Success}, Failures: {Failures}",
            request.CourseId,
            request.StudentMarks.Count,
            successCount,
            errors.Count);

        return new BulkUpdateCourseEnrollmentMarksResult(
            request.StudentMarks.Count,
            successCount,
            errors.Count,
            errors);
    }
}


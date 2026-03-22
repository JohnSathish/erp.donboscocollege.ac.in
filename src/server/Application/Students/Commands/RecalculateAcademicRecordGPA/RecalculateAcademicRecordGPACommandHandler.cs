using ERP.Application.Students.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.RecalculateAcademicRecordGPA;

public sealed class RecalculateAcademicRecordGPACommandHandler : IRequestHandler<RecalculateAcademicRecordGPACommand, RecalculateAcademicRecordGPAResult>
{
    private readonly IAcademicHistoryRepository _academicHistoryRepository;
    private readonly IGradeCalculationService _gradeCalculationService;
    private readonly ILogger<RecalculateAcademicRecordGPACommandHandler> _logger;

    public RecalculateAcademicRecordGPACommandHandler(
        IAcademicHistoryRepository academicHistoryRepository,
        IGradeCalculationService gradeCalculationService,
        ILogger<RecalculateAcademicRecordGPACommandHandler> logger)
    {
        _academicHistoryRepository = academicHistoryRepository;
        _gradeCalculationService = gradeCalculationService;
        _logger = logger;
    }

    public async Task<RecalculateAcademicRecordGPAResult> Handle(RecalculateAcademicRecordGPACommand request, CancellationToken cancellationToken)
    {
        var academicRecord = await _academicHistoryRepository.GetAcademicRecordByIdAsync(request.AcademicRecordId, cancellationToken);
        if (academicRecord == null)
        {
            return new RecalculateAcademicRecordGPAResult(
                request.AcademicRecordId,
                null,
                null,
                false,
                "Academic record not found.");
        }

        try
        {
            var result = await _gradeCalculationService.CalculateGPAForAcademicRecordAsync(
                request.AcademicRecordId,
                cancellationToken);

            if (!result.Success)
            {
                return new RecalculateAcademicRecordGPAResult(
                    request.AcademicRecordId,
                    null,
                    null,
                    false,
                    result.ErrorMessage);
            }

            academicRecord.UpdateResults(
                result.GPA,
                null, // CGPA calculated separately
                result.Grade,
                result.CreditsEarned >= result.TotalCredits * 0.4m ? "Pass" : "Fail",
                result.TotalCredits,
                result.CreditsEarned,
                null,
                request.UpdatedBy);

            await _academicHistoryRepository.UpdateAsync(academicRecord, cancellationToken);

            _logger.LogInformation(
                "Recalculated GPA for academic record {AcademicRecordId}, New GPA: {GPA}, Grade: {Grade}",
                request.AcademicRecordId,
                result.GPA,
                result.Grade);

            return new RecalculateAcademicRecordGPAResult(
                request.AcademicRecordId,
                result.GPA,
                result.Grade,
                true);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to recalculate GPA for academic record {AcademicRecordId}",
                request.AcademicRecordId);

            return new RecalculateAcademicRecordGPAResult(
                request.AcademicRecordId,
                null,
                null,
                false,
                ex.Message);
        }
    }
}


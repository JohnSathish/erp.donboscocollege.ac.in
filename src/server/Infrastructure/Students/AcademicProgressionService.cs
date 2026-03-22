using ERP.Application.Admissions.Interfaces;
using ERP.Application.Students.Interfaces;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Students;

public class AcademicProgressionService : IAcademicProgressionService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IAcademicHistoryRepository _academicHistoryRepository;
    private readonly IGradeCalculationService _gradeCalculationService;
    private readonly IProgramRepository _programRepository;
    private readonly ILogger<AcademicProgressionService> _logger;

    // Configuration constants - these should ideally come from configuration
    private const decimal MinimumGPAPromotion = 4.0m; // Minimum GPA for promotion
    private const decimal MinimumGPAGraduation = 5.0m; // Minimum CGPA for graduation
    private const decimal MinimumPassPercentage = 40.0m; // Minimum percentage to pass a course

    public AcademicProgressionService(
        IStudentRepository studentRepository,
        IAcademicHistoryRepository academicHistoryRepository,
        IGradeCalculationService gradeCalculationService,
        IProgramRepository programRepository,
        ILogger<AcademicProgressionService> logger)
    {
        _studentRepository = studentRepository;
        _academicHistoryRepository = academicHistoryRepository;
        _gradeCalculationService = gradeCalculationService;
        _programRepository = programRepository;
        _logger = logger;
    }

    public async Task<ProgressionEvaluationResult> EvaluateProgressionAsync(
        Guid studentId,
        Guid academicRecordId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
            if (student == null)
            {
                return new ProgressionEvaluationResult(
                    studentId,
                    academicRecordId,
                    "Retain",
                    "Student not found.",
                    false,
                    null,
                    MinimumGPAPromotion,
                    0,
                    0,
                    false,
                    "Student not found.");
            }

            var academicRecord = await _academicHistoryRepository.GetAcademicRecordByIdAsync(academicRecordId, cancellationToken);
            if (academicRecord == null)
            {
                return new ProgressionEvaluationResult(
                    studentId,
                    academicRecordId,
                    "Retain",
                    "Academic record not found.",
                    false,
                    null,
                    MinimumGPAPromotion,
                    0,
                    0,
                    false,
                    "Academic record not found.");
            }

            var enrollments = await _academicHistoryRepository.GetCourseEnrollmentsByStudentIdAsync(
                studentId,
                academicRecord.TermId,
                cancellationToken);

            var recordEnrollments = enrollments
                .Where(e => e.AcademicRecordId == academicRecordId && e.IsCompleted)
                .ToList();

            var gpaResult = await _gradeCalculationService.CalculateGPAForAcademicRecordAsync(academicRecordId, cancellationToken);
            var currentGPA = gpaResult.GPA ?? academicRecord.GPA;

            // Get program requirements
            var program = student.ProgramId.HasValue
                ? await _programRepository.GetByIdAsync(student.ProgramId.Value, cancellationToken)
                : null;

            var creditsRequired = program?.TotalCredits ?? 0; // This should be per semester, but using total as placeholder
            var creditsEarned = academicRecord.CreditsEarned;

            string recommendation;
            string reason;
            bool isEligible;

            if (currentGPA.HasValue)
            {
                if (currentGPA >= MinimumGPAPromotion)
                {
                    // Check if all courses are passed
                    var failedCourses = recordEnrollments.Count(e =>
                        e.MarksObtained.HasValue && e.MaxMarks.HasValue &&
                        (e.MarksObtained.Value / e.MaxMarks.Value) * 100 < MinimumPassPercentage);

                    if (failedCourses == 0)
                    {
                        recommendation = "Promote";
                        reason = $"GPA {currentGPA:F2} meets minimum requirement of {MinimumGPAPromotion:F2}. All courses passed.";
                        isEligible = true;
                    }
                    else
                    {
                        recommendation = "Conditional";
                        reason = $"GPA {currentGPA:F2} meets requirement, but {failedCourses} course(s) failed. Conditional promotion may be considered.";
                        isEligible = false;
                    }
                }
                else
                {
                    recommendation = "Retain";
                    reason = $"GPA {currentGPA:F2} is below minimum requirement of {MinimumGPAPromotion:F2}.";
                    isEligible = false;
                }
            }
            else
            {
                recommendation = "Retain";
                reason = "GPA not calculated. Cannot determine progression eligibility.";
                isEligible = false;
            }

            return new ProgressionEvaluationResult(
                studentId,
                academicRecordId,
                recommendation,
                reason,
                isEligible,
                currentGPA,
                MinimumGPAPromotion,
                creditsEarned,
                creditsRequired,
                true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to evaluate progression for student {StudentId}, academic record {AcademicRecordId}", studentId, academicRecordId);
            return new ProgressionEvaluationResult(
                studentId,
                academicRecordId,
                "Retain",
                "Error during evaluation.",
                false,
                null,
                MinimumGPAPromotion,
                0,
                0,
                false,
                ex.Message);
        }
    }

    public async Task<GraduationEligibilityResult> CheckGraduationEligibilityAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
            if (student == null)
            {
                return new GraduationEligibilityResult(
                    studentId,
                    false,
                    "Student not found.",
                    null,
                    MinimumGPAGraduation,
                    0,
                    0,
                    Array.Empty<string>(),
                    false,
                    "Student not found.");
            }

            var program = student.ProgramId.HasValue
                ? await _programRepository.GetByIdAsync(student.ProgramId.Value, cancellationToken)
                : null;

            if (program == null)
            {
                return new GraduationEligibilityResult(
                    studentId,
                    false,
                    "Program information not found.",
                    null,
                    MinimumGPAGraduation,
                    0,
                    0,
                    new[] { "Program information missing" },
                    false,
                    "Program not found.");
            }

            var cgpaResult = await _gradeCalculationService.CalculateCGPAForStudentAsync(studentId, cancellationToken);
            var currentCGPA = cgpaResult.CGPA;

            var academicRecords = await _academicHistoryRepository.GetAcademicRecordsByStudentIdAsync(studentId, cancellationToken);
            var totalCreditsEarned = academicRecords.Sum(r => r.CreditsEarned);
            var totalCreditsRequired = program.TotalCredits;

            var pendingRequirements = new List<string>();

            if (!currentCGPA.HasValue || currentCGPA < MinimumGPAGraduation)
            {
                pendingRequirements.Add($"CGPA {currentCGPA:F2 ?? 0:F2} is below minimum requirement of {MinimumGPAGraduation:F2}");
            }

            if (totalCreditsEarned < totalCreditsRequired)
            {
                pendingRequirements.Add($"Credits earned {totalCreditsEarned} is below required {totalCreditsRequired}");
            }

            // Check if all semesters are completed
            var incompleteSemesters = academicRecords.Count(r => r.ResultStatus != "Pass");
            if (incompleteSemesters > 0)
            {
                pendingRequirements.Add($"{incompleteSemesters} semester(s) not completed successfully");
            }

            var isEligible = pendingRequirements.Count == 0;
            var reason = isEligible
                ? $"CGPA {currentCGPA:F2} and {totalCreditsEarned} credits meet graduation requirements."
                : string.Join("; ", pendingRequirements);

            return new GraduationEligibilityResult(
                studentId,
                isEligible,
                reason,
                currentCGPA,
                MinimumGPAGraduation,
                totalCreditsEarned,
                totalCreditsRequired,
                pendingRequirements,
                true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check graduation eligibility for student {StudentId}", studentId);
            return new GraduationEligibilityResult(
                studentId,
                false,
                "Error during evaluation.",
                null,
                MinimumGPAGraduation,
                0,
                0,
                new[] { "System error occurred" },
                false,
                ex.Message);
        }
    }
}


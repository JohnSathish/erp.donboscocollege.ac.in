namespace ERP.Application.Students.Interfaces;

public interface IAcademicProgressionService
{
    Task<ProgressionEvaluationResult> EvaluateProgressionAsync(
        Guid studentId,
        Guid academicRecordId,
        CancellationToken cancellationToken = default);

    Task<GraduationEligibilityResult> CheckGraduationEligibilityAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);
}

public sealed record ProgressionEvaluationResult(
    Guid StudentId,
    Guid AcademicRecordId,
    string Recommendation, // "Promote", "Retain", "Conditional"
    string Reason,
    bool IsEligibleForPromotion,
    decimal? CurrentGPA,
    decimal? RequiredGPA,
    int CreditsEarned,
    int CreditsRequired,
    bool Success,
    string? ErrorMessage = null);

public sealed record GraduationEligibilityResult(
    Guid StudentId,
    bool IsEligible,
    string? Reason,
    decimal? CurrentCGPA,
    decimal? RequiredCGPA,
    int TotalCreditsEarned,
    int TotalCreditsRequired,
    IReadOnlyCollection<string> PendingRequirements,
    bool Success,
    string? ErrorMessage = null);


namespace ERP.Application.Admissions.ViewModels;

public sealed record ProgramDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    string Level,
    int DurationYears,
    int TotalCredits,
    bool IsActive,
    DateTime CreatedOnUtc,
    string? CreatedBy,
    DateTime? UpdatedOnUtc,
    string? UpdatedBy);










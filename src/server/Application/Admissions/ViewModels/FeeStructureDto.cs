namespace ERP.Application.Admissions.ViewModels;

public sealed record FeeStructureDto(
    Guid Id,
    string Name,
    string? Description,
    Guid? ProgramId,
    string? ProgramName,
    string AcademicYear,
    bool IsActive,
    DateTime EffectiveFromUtc,
    DateTime? EffectiveToUtc,
    decimal TotalAmount,
    IReadOnlyCollection<FeeComponentDto> Components,
    DateTime CreatedOnUtc,
    string? CreatedBy,
    DateTime? UpdatedOnUtc,
    string? UpdatedBy);










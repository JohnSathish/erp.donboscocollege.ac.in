namespace ERP.Application.Admissions.ViewModels;

public sealed record CourseDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    Guid? ProgramId,
    string? ProgramName,
    int CreditHours,
    string? Prerequisites,
    bool IsActive,
    DateTime CreatedOnUtc,
    string? CreatedBy,
    DateTime? UpdatedOnUtc,
    string? UpdatedBy);










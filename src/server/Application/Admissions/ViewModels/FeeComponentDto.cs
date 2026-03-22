namespace ERP.Application.Admissions.ViewModels;

public sealed record FeeComponentDto(
    Guid Id,
    string Name,
    string? Description,
    decimal Amount,
    bool IsOptional,
    int? InstallmentNumber,
    DateTime? DueDateUtc,
    int DisplayOrder,
    DateTime CreatedOnUtc,
    string? CreatedBy,
    DateTime? UpdatedOnUtc,
    string? UpdatedBy);










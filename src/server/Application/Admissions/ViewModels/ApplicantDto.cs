namespace ERP.Application.Admissions.ViewModels;

public sealed record ApplicantDto(
    Guid Id,
    string ApplicationNumber,
    string FullName,
    string Email,
    string MobileNumber,
    DateOnly DateOfBirth,
    string ProgramCode,
    string Status,
    DateTime StatusUpdatedOnUtc,
    string? StatusUpdatedBy,
    string? StatusRemarks,
    DateTime? EntranceExamScheduledOnUtc,
    string? EntranceExamVenue,
    string? EntranceExamInstructions,
    DateTime CreatedOnUtc);



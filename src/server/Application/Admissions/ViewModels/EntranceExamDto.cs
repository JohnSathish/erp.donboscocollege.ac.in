namespace ERP.Application.Admissions.ViewModels;

public sealed record EntranceExamDto(
    Guid Id,
    string ExamName,
    string ExamCode,
    string? Description,
    DateTime ExamDate,
    TimeOnly ExamStartTime,
    TimeOnly ExamEndTime,
    string Venue,
    string? VenueAddress,
    string? Instructions,
    int MaxCapacity,
    int CurrentRegistrations,
    bool IsActive,
    DateTime CreatedOnUtc,
    string? CreatedBy,
    DateTime? UpdatedOnUtc,
    string? UpdatedBy);














namespace ERP.Application.Admissions.ViewModels;

public sealed record ExamRegistrationDto(
    Guid Id,
    Guid ExamId,
    string ExamName,
    string ExamCode,
    Guid ApplicantAccountId,
    string ApplicantName,
    string ApplicantUniqueId,
    string? HallTicketNumber,
    bool IsPresent,
    decimal? Score,
    DateTime RegisteredOnUtc,
    string? RegisteredBy,
    DateTime? AttendanceMarkedOnUtc,
    string? AttendanceMarkedBy,
    DateTime? ScoreEnteredOnUtc,
    string? ScoreEnteredBy);














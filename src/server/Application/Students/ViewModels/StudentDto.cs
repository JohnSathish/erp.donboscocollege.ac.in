namespace ERP.Application.Students.ViewModels;

public sealed record StudentDto(
    Guid Id,
    string StudentNumber,
    string FullName,
    DateOnly DateOfBirth,
    string Gender,
    string Email,
    string MobileNumber,
    string? PhotoUrl,
    Guid? ProgramId,
    string? ProgramCode,
    string? MajorSubject,
    string? MinorSubject,
    string Shift,
    string AcademicYear,
    string? AdmissionNumber,
    DateTime EnrollmentDate,
    string Status,
    DateTime CreatedOnUtc,
    string? CreatedBy,
    DateTime? UpdatedOnUtc,
    string? UpdatedBy);



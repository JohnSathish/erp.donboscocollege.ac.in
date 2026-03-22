using MediatR;

namespace ERP.Application.Students.Queries.GetStudentWithGuardians;

public sealed record GetStudentWithGuardiansQuery(Guid StudentId) : IRequest<StudentWithGuardiansDto?>;

public sealed record StudentWithGuardiansDto(
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
    IReadOnlyCollection<GuardianDto> Guardians);

public sealed record GuardianDto(
    Guid Id,
    string Name,
    string Relationship,
    string? Age,
    string? Occupation,
    string ContactNumber,
    string? Email,
    bool IsPrimary,
    bool IsActive);


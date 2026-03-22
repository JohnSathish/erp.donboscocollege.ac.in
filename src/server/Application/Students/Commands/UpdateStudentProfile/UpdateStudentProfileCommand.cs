using MediatR;

namespace ERP.Application.Students.Commands.UpdateStudentProfile;

public sealed record UpdateStudentProfileCommand(
    Guid StudentId,
    string FullName,
    string Email,
    string MobileNumber,
    string Shift,
    Guid? ProgramId = null,
    string? ProgramCode = null,
    string? MajorSubject = null,
    string? MinorSubject = null,
    string? PhotoUrl = null,
    string? UpdatedBy = null) : IRequest<UpdateStudentProfileResult>;

public sealed record UpdateStudentProfileResult(
    Guid StudentId,
    string StudentNumber,
    bool Success,
    string? ErrorMessage = null);


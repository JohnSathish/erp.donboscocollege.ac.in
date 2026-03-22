using MediatR;

namespace ERP.Application.Admissions.Commands.ResetStudentApplicantPassword;

public sealed record ResetStudentApplicantPasswordCommand(
    string ApplicationNumber,
    string? ResetBy = null) : IRequest<ResetStudentApplicantPasswordResult>;

public sealed record ResetStudentApplicantPasswordResult(
    string ApplicationNumber,
    string FullName,
    string Email,
    string TemporaryPassword);


namespace ERP.Application.Admissions.Commands.LoginStudentApplicant;

public sealed record LoginStudentApplicantResult(
    string Token,
    DateTime ExpiresAtUtc,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUtc,
    string UniqueId,
    string Email,
    string FullName,
    bool MustChangePassword);


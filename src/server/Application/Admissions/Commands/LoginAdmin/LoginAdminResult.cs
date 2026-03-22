namespace ERP.Application.Admissions.Commands.LoginAdmin;

public sealed record LoginAdminResult(
    string Token,
    DateTime ExpiresAtUtc,
    string UniqueId,
    string Email,
    string FullName);















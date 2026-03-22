namespace ERP.Application.Common.Interfaces;

public interface IUserAccountService
{
    Task<CreateUserAccountResult> CreateStudentAccountAsync(
        Guid studentId,
        string studentNumber,
        string fullName,
        string email,
        string mobileNumber,
        CancellationToken cancellationToken = default);

    Task<CreateUserAccountResult> CreateParentAccountAsync(
        Guid guardianId,
        string guardianName,
        string relationship,
        string contactNumber,
        string? email,
        Guid studentId,
        CancellationToken cancellationToken = default);

    Task<bool> SendCredentialsAsync(
        string email,
        string mobileNumber,
        string username,
        string temporaryPassword,
        string userType,
        CancellationToken cancellationToken = default);
}

public sealed record CreateUserAccountResult(
    Guid UserId,
    string Username,
    string TemporaryPassword,
    bool Success,
    string? ErrorMessage = null);


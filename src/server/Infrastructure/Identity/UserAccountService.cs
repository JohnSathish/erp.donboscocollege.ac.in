using ERP.Application.Common.Interfaces;
using ERP.Application.Admissions.Interfaces;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Identity;

public class UserAccountService : IUserAccountService
{
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<UserAccountService> _logger;

    public UserAccountService(
        IPasswordGenerator passwordGenerator,
        IApplicantNotificationService notificationService,
        ILogger<UserAccountService> logger)
    {
        _passwordGenerator = passwordGenerator;
        _notificationService = notificationService;
        _logger = logger;
    }

    public Task<CreateUserAccountResult> CreateStudentAccountAsync(
        Guid studentId,
        string studentNumber,
        string fullName,
        string email,
        string mobileNumber,
        CancellationToken cancellationToken = default)
    {
        // TODO: Integrate with ASP.NET Core Identity when Identity module is fully implemented
        // For now, generate credentials and send notifications
        // In production, this should create actual IdentityUser records

        try
        {
            var temporaryPassword = _passwordGenerator.GenerateTemporaryPassword();
            var username = email; // Use email as username

            _logger.LogInformation(
                "Creating student account for StudentId: {StudentId}, StudentNumber: {StudentNumber}, Email: {Email}",
                studentId,
                studentNumber,
                email);

            // TODO: Create IdentityUser in ASP.NET Core Identity
            // var user = new IdentityUser { UserName = username, Email = email };
            // var result = await _userManager.CreateAsync(user, temporaryPassword);
            // if (!result.Succeeded) { ... }

            // For now, generate a placeholder UserId
            var userId = Guid.NewGuid();

            return Task.FromResult(new CreateUserAccountResult(
                userId,
                username,
                temporaryPassword,
                true));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to create student account for StudentId: {StudentId}, Email: {Email}",
                studentId,
                email);

            return Task.FromResult(new CreateUserAccountResult(
                Guid.Empty,
                string.Empty,
                string.Empty,
                false,
                ex.Message));
        }
    }

    public Task<CreateUserAccountResult> CreateParentAccountAsync(
        Guid guardianId,
        string guardianName,
        string relationship,
        string contactNumber,
        string? email,
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        // TODO: Integrate with ASP.NET Core Identity when Identity module is fully implemented
        // For now, generate credentials and send notifications
        // In production, this should create actual IdentityUser records

        try
        {
            var temporaryPassword = _passwordGenerator.GenerateTemporaryPassword();
            var username = email ?? contactNumber; // Use email if available, otherwise contact number

            _logger.LogInformation(
                "Creating parent account for GuardianId: {GuardianId}, Name: {Name}, Relationship: {Relationship}",
                guardianId,
                guardianName,
                relationship);

            // TODO: Create IdentityUser in ASP.NET Core Identity
            // var user = new IdentityUser { UserName = username, Email = email ?? $"{contactNumber}@guardian.local" };
            // var result = await _userManager.CreateAsync(user, temporaryPassword);
            // if (!result.Succeeded) { ... }

            // For now, generate a placeholder UserId
            var userId = Guid.NewGuid();

            return Task.FromResult(new CreateUserAccountResult(
                userId,
                username,
                temporaryPassword,
                true));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to create parent account for GuardianId: {GuardianId}, Name: {Name}",
                guardianId,
                guardianName);

            return Task.FromResult(new CreateUserAccountResult(
                Guid.Empty,
                string.Empty,
                string.Empty,
                false,
                ex.Message));
        }
    }

    public async Task<bool> SendCredentialsAsync(
        string email,
        string mobileNumber,
        string username,
        string temporaryPassword,
        string userType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Use the existing notification service to send credentials
            // This reuses the registration notification flow
            await _notificationService.SendRegistrationNotificationsAsync(
                username, // Use username as fullName for now
                username, // Use username as uniqueId
                email,
                mobileNumber,
                temporaryPassword,
                cancellationToken);

            _logger.LogInformation(
                "Credentials sent successfully to {Email} for {UserType} account",
                email,
                userType);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send credentials to {Email} for {UserType} account",
                email,
                userType);

            return false;
        }
    }
}


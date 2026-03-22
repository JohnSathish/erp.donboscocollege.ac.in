using ERP.Application.Admissions.Interfaces;
using ERP.Application.Common.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.ResetStudentApplicantPassword;

public sealed class ResetStudentApplicantPasswordCommandHandler : IRequestHandler<ResetStudentApplicantPasswordCommand, ResetStudentApplicantPasswordResult>
{
    private readonly IApplicantAccountRepository _repository;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IApplicantPasswordHasher _passwordHasher;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<ResetStudentApplicantPasswordCommandHandler> _logger;

    public ResetStudentApplicantPasswordCommandHandler(
        IApplicantAccountRepository repository,
        IPasswordGenerator passwordGenerator,
        IApplicantPasswordHasher passwordHasher,
        IApplicantNotificationService notificationService,
        ILogger<ResetStudentApplicantPasswordCommandHandler> logger)
    {
        _repository = repository;
        _passwordGenerator = passwordGenerator;
        _passwordHasher = passwordHasher;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<ResetStudentApplicantPasswordResult> Handle(ResetStudentApplicantPasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Resetting password for applicant account {ApplicationNumber} by {ResetBy}",
            request.ApplicationNumber,
            request.ResetBy ?? "System");

        var account = await _repository.GetByUniqueIdAsync(request.ApplicationNumber, cancellationToken);
        if (account is null)
        {
            throw new InvalidOperationException($"Applicant account with application number {request.ApplicationNumber} not found.");
        }

        // Generate new temporary password
        var tempPassword = _passwordGenerator.GenerateTemporaryPassword();
        var passwordHash = _passwordHasher.HashPassword(account, tempPassword);
        
        // Update password and mark as must change
        await _repository.UpdatePasswordAsync(account.Id, passwordHash, mustChangePassword: true, cancellationToken);
        
        // Revoke all existing refresh tokens for security
        await _repository.RevokeRefreshTokensByAccountAsync(account.Id, cancellationToken);

        // Send notification with new temporary password
        try
        {
            await _notificationService.SendPasswordResetNotificationAsync(
                account.FullName,
                account.Email,
                account.MobileNumber,
                account.UniqueId,
                tempPassword,
                request.ResetBy ?? "System Administrator",
                cancellationToken);

            _logger.LogInformation(
                "Password reset notification sent to {Email} for application {ApplicationNumber}",
                account.Email,
                account.UniqueId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send password reset notification to {Email} for application {ApplicationNumber}",
                account.Email,
                account.UniqueId);
            // Don't fail the command if notification fails - password is still reset
        }

        return new ResetStudentApplicantPasswordResult(
            account.UniqueId,
            account.FullName,
            account.Email,
            tempPassword);
    }
}


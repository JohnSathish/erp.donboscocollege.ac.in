using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.ConfirmAdmissionFeePayment;

public sealed class ConfirmAdmissionFeePaymentCommandHandler
    : IRequestHandler<ConfirmAdmissionFeePaymentCommand, ConfirmAdmissionFeePaymentResult>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<ConfirmAdmissionFeePaymentCommandHandler> _logger;

    public ConfirmAdmissionFeePaymentCommandHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantNotificationService notificationService,
        ILogger<ConfirmAdmissionFeePaymentCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<ConfirmAdmissionFeePaymentResult> Handle(
        ConfirmAdmissionFeePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdForUpdateAsync(request.AccountId, cancellationToken)
                      ?? throw new InvalidOperationException($"Account {request.AccountId} not found.");

        if (account.Status != ApplicationStatus.DirectAdmissionGranted)
        {
            throw new InvalidOperationException(
                $"Admission fee can only be confirmed when status is DirectAdmissionGranted (current: {account.Status}).");
        }

        account.UpdateStatus(
            ApplicationStatus.AdmissionFeePaid,
            request.PerformedBy,
            DateTime.UtcNow,
            "Admission fee payment recorded (direct admission path).");

        await _accountRepository.UpdateAsync(account, cancellationToken);

        _logger.LogInformation(
            "AUDIT AdmissionFeePaid accountId={AccountId} uniqueId={UniqueId} by={By}",
            account.Id,
            account.UniqueId,
            request.PerformedBy ?? "unknown");

        if (request.NotifyApplicant)
        {
            try
            {
                await _notificationService.SendBulkEmailAsync(
                    account.FullName,
                    account.Email,
                    "Admission fee received — Don Bosco College Tura",
                    $"<p>Dear {account.FullName},</p><p>We have received your admission fee payment for application <strong>{account.UniqueId}</strong>. " +
                    "Our admissions team will finalize your admission. You will receive further instructions shortly.</p>",
                    cancellationToken);

                await _notificationService.SendBulkSmsAsync(
                    account.MobileNumber,
                    $"Dear applicant, your admission fee for {account.UniqueId} has been received. — Admissions, DBCT",
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send admission fee paid notifications for {AccountId}", account.Id);
            }
        }

        return new ConfirmAdmissionFeePaymentResult(account.Id, ApplicationStatus.AdmissionFeePaid.ToString());
    }
}

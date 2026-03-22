using ERP.Application.Admissions.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.RejectAdmissionOffer;

public sealed class RejectAdmissionOfferCommandHandler : IRequestHandler<RejectAdmissionOfferCommand, RejectAdmissionOfferResult>
{
    private readonly IAdmissionsRepository _admissionsRepository;
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<RejectAdmissionOfferCommandHandler> _logger;

    public RejectAdmissionOfferCommandHandler(
        IAdmissionsRepository admissionsRepository,
        IApplicantAccountRepository accountRepository,
        IApplicantNotificationService notificationService,
        ILogger<RejectAdmissionOfferCommandHandler> logger)
    {
        _admissionsRepository = admissionsRepository;
        _accountRepository = accountRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<RejectAdmissionOfferResult> Handle(RejectAdmissionOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _admissionsRepository.GetOfferByIdAsync(request.OfferId, cancellationToken);
        if (offer == null)
        {
            return new RejectAdmissionOfferResult(
                request.OfferId,
                string.Empty,
                false,
                "Offer not found.");
        }

        try
        {
            offer.Reject(request.Reason);
            await _admissionsRepository.UpdateAdmissionOfferAsync(offer, cancellationToken);

            // Get account for notification
            var account = await _accountRepository.GetByIdAsync(offer.AccountId, cancellationToken);
            
            // Send notification
            if (account != null)
            {
                try
                {
                    await _notificationService.SendOfferRejectedNotificationAsync(
                        account.FullName,
                        account.Email,
                        account.MobileNumber,
                        offer.ApplicationNumber,
                        DateTime.UtcNow,
                        request.Reason,
                        cancellationToken);

                    _logger.LogInformation(
                        "Offer rejected notification sent to {Email} for application {ApplicationNumber}",
                        account.Email,
                        offer.ApplicationNumber);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to send offer rejected notification to {Email} for application {ApplicationNumber}",
                        account.Email,
                        offer.ApplicationNumber);
                    // Don't fail the command if notification fails
                }
            }

            return new RejectAdmissionOfferResult(
                offer.Id,
                offer.ApplicationNumber,
                true,
                "Admission offer rejected successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return new RejectAdmissionOfferResult(
                offer.Id,
                offer.ApplicationNumber,
                false,
                ex.Message);
        }
    }
}


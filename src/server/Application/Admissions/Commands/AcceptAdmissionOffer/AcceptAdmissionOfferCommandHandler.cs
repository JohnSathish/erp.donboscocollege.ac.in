using ERP.Application.Admissions.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.AcceptAdmissionOffer;

public sealed class AcceptAdmissionOfferCommandHandler : IRequestHandler<AcceptAdmissionOfferCommand, AcceptAdmissionOfferResult>
{
    private readonly IAdmissionsRepository _admissionsRepository;
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<AcceptAdmissionOfferCommandHandler> _logger;

    public AcceptAdmissionOfferCommandHandler(
        IAdmissionsRepository admissionsRepository,
        IApplicantAccountRepository accountRepository,
        IApplicantNotificationService notificationService,
        ILogger<AcceptAdmissionOfferCommandHandler> logger)
    {
        _admissionsRepository = admissionsRepository;
        _accountRepository = accountRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<AcceptAdmissionOfferResult> Handle(AcceptAdmissionOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _admissionsRepository.GetOfferByIdAsync(request.OfferId, cancellationToken);
        if (offer == null)
        {
            return new AcceptAdmissionOfferResult(
                request.OfferId,
                string.Empty,
                false,
                "Offer not found.");
        }

        // Check if offer is expired
        offer.MarkExpired();
        if (offer.Status == Domain.Admissions.Entities.OfferStatus.Expired)
        {
            await _admissionsRepository.UpdateAdmissionOfferAsync(offer, cancellationToken);
            return new AcceptAdmissionOfferResult(
                offer.Id,
                offer.ApplicationNumber,
                false,
                "This offer has expired.");
        }

        try
        {
            offer.Accept();
            await _admissionsRepository.UpdateAdmissionOfferAsync(offer, cancellationToken);

            // Get account for notification
            var account = await _accountRepository.GetByIdAsync(offer.AccountId, cancellationToken);
            
            // Send notification
            if (account != null)
            {
                try
                {
                    await _notificationService.SendOfferAcceptedNotificationAsync(
                        account.FullName,
                        account.Email,
                        account.MobileNumber,
                        offer.ApplicationNumber,
                        DateTime.UtcNow,
                        cancellationToken);

                    _logger.LogInformation(
                        "Offer accepted notification sent to {Email} for application {ApplicationNumber}",
                        account.Email,
                        offer.ApplicationNumber);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to send offer accepted notification to {Email} for application {ApplicationNumber}",
                        account.Email,
                        offer.ApplicationNumber);
                    // Don't fail the command if notification fails
                }
            }

            return new AcceptAdmissionOfferResult(
                offer.Id,
                offer.ApplicationNumber,
                true,
                "Admission offer accepted successfully. Please check your email for further instructions.");
        }
        catch (InvalidOperationException ex)
        {
            return new AcceptAdmissionOfferResult(
                offer.Id,
                offer.ApplicationNumber,
                false,
                ex.Message);
        }
    }
}


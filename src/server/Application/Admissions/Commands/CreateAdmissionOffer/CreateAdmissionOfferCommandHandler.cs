using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.CreateAdmissionOffer;

public sealed class CreateAdmissionOfferCommandHandler : IRequestHandler<CreateAdmissionOfferCommand, CreateAdmissionOfferResult>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IAdmissionsRepository _admissionsRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<CreateAdmissionOfferCommandHandler> _logger;

    public CreateAdmissionOfferCommandHandler(
        IApplicantAccountRepository accountRepository,
        IAdmissionsRepository admissionsRepository,
        IApplicantNotificationService notificationService,
        ILogger<CreateAdmissionOfferCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _admissionsRepository = admissionsRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<CreateAdmissionOfferResult> Handle(CreateAdmissionOfferCommand request, CancellationToken cancellationToken)
    {
        // Get account
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account == null)
        {
            throw new InvalidOperationException($"Account with ID {request.AccountId} not found.");
        }

        // Check if account already has an offer
        var existingOffer = await _admissionsRepository.GetOfferByAccountIdAsync(request.AccountId, cancellationToken);
        if (existingOffer != null && existingOffer.Status == OfferStatus.Pending)
        {
            throw new InvalidOperationException($"Account {account.UniqueId} already has a pending offer.");
        }

        // Get merit score
        var meritScore = await _admissionsRepository.GetMeritScoreByAccountIdAsync(request.AccountId, cancellationToken);
        if (meritScore == null)
        {
            throw new InvalidOperationException($"Merit score not found for account {account.UniqueId}. Please generate merit list first.");
        }

        // Create offer
        var offer = new AdmissionOffer(
            account.Id,
            account.UniqueId,
            account.FullName,
            meritScore.Rank,
            meritScore.Shift,
            meritScore.MajorSubject,
            request.ExpiryDate,
            request.CreatedBy ?? "System",
            request.Remarks);

        await _admissionsRepository.AddAdmissionOfferAsync(offer, cancellationToken);

        // Send notification
        try
        {
            await _notificationService.SendAdmissionOfferNotificationAsync(
                account.FullName,
                account.Email,
                account.MobileNumber,
                offer.ApplicationNumber,
                offer.MeritRank,
                offer.Shift,
                offer.MajorSubject,
                offer.OfferDate,
                offer.ExpiryDate,
                cancellationToken);

            _logger.LogInformation(
                "Admission offer notification sent to {Email} for application {ApplicationNumber}",
                account.Email,
                offer.ApplicationNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send admission offer notification to {Email} for application {ApplicationNumber}",
                account.Email,
                offer.ApplicationNumber);
            // Don't fail the command if notification fails
        }

        return new CreateAdmissionOfferResult(
            offer.Id,
            offer.ApplicationNumber,
            offer.FullName,
            offer.MeritRank,
            offer.OfferDate,
            offer.ExpiryDate);
    }
}


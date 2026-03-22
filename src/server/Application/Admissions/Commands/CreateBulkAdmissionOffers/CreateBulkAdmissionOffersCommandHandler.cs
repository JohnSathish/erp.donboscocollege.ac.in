using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.CreateBulkAdmissionOffers;

public sealed class CreateBulkAdmissionOffersCommandHandler : IRequestHandler<CreateBulkAdmissionOffersCommand, CreateBulkAdmissionOffersResult>
{
    private readonly IAdmissionsRepository _admissionsRepository;
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<CreateBulkAdmissionOffersCommandHandler> _logger;

    public CreateBulkAdmissionOffersCommandHandler(
        IAdmissionsRepository admissionsRepository,
        IApplicantAccountRepository accountRepository,
        IApplicantNotificationService notificationService,
        ILogger<CreateBulkAdmissionOffersCommandHandler> logger)
    {
        _admissionsRepository = admissionsRepository;
        _accountRepository = accountRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<CreateBulkAdmissionOffersResult> Handle(CreateBulkAdmissionOffersCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating bulk admission offers. Shift: {Shift}, MajorSubject: {MajorSubject}, TopNRanks: {TopNRanks}",
            request.Shift,
            request.MajorSubject,
            request.TopNRanks);

        // Get merit scores based on filters
        var meritScores = await _admissionsRepository.GetMeritScoresForBulkOffersAsync(
            request.Shift,
            request.MajorSubject,
            request.TopNRanks,
            cancellationToken);

        if (meritScores.Count == 0)
        {
            _logger.LogWarning("No merit scores found matching the criteria.");
            return new CreateBulkAdmissionOffersResult(0, new List<Guid>(), new List<string> { "No merit scores found matching the criteria." });
        }

        var defaultExpiryDate = request.ExpiryDate ?? DateTime.UtcNow.AddDays(30);
        var createdOfferIds = new List<Guid>();
        var errors = new List<string>();

        foreach (var meritScore in meritScores)
        {
            try
            {
                // Check if offer already exists
                var existingOffer = await _admissionsRepository.GetOfferByAccountIdAsync(meritScore.AccountId, cancellationToken);
                if (existingOffer != null && existingOffer.Status == OfferStatus.Pending)
                {
                    _logger.LogInformation(
                        "Skipping account {AccountId} - already has a pending offer.",
                        meritScore.AccountId);
                    continue;
                }

                // Get account details
                var account = await _accountRepository.GetByIdAsync(meritScore.AccountId, cancellationToken);
                if (account == null)
                {
                    errors.Add($"Account not found for merit score ID {meritScore.Id}");
                    continue;
                }

                // Create offer
                var offer = new AdmissionOffer(
                    account.Id,
                    account.UniqueId,
                    account.FullName,
                    meritScore.Rank,
                    meritScore.Shift,
                    meritScore.MajorSubject,
                    defaultExpiryDate,
                    request.CreatedBy ?? "System",
                    null);

                await _admissionsRepository.AddAdmissionOfferAsync(offer, cancellationToken);
                createdOfferIds.Add(offer.Id);

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
                    // Don't fail the bulk operation if notification fails
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"Failed to create offer for merit score ID {meritScore.Id}: {ex.Message}";
                errors.Add(errorMsg);
                _logger.LogError(ex, errorMsg);
            }
        }

        _logger.LogInformation(
            "Bulk offer creation complete. Created {Count} offers, {ErrorCount} errors.",
            createdOfferIds.Count,
            errors.Count);

        return new CreateBulkAdmissionOffersResult(createdOfferIds.Count, createdOfferIds, errors);
    }
}


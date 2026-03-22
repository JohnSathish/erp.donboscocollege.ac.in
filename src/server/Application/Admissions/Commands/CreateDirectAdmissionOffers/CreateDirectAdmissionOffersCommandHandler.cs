using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.CreateDirectAdmissionOffers;

public sealed class CreateDirectAdmissionOffersCommandHandler : IRequestHandler<CreateDirectAdmissionOffersCommand, CreateDirectAdmissionOffersResult>
{
    private readonly IAdmissionsRepository _admissionsRepository;
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<CreateDirectAdmissionOffersCommandHandler> _logger;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public CreateDirectAdmissionOffersCommandHandler(
        IAdmissionsRepository admissionsRepository,
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository,
        IApplicantNotificationService notificationService,
        ILogger<CreateDirectAdmissionOffersCommandHandler> logger)
    {
        _admissionsRepository = admissionsRepository;
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<CreateDirectAdmissionOffersResult> Handle(CreateDirectAdmissionOffersCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating direct admission offers for applicants with percentage >= {MinimumPercentage}%. Admission fee: ₹{AdmissionFee}",
            request.MinimumPercentage,
            request.AdmissionFeeAmount);

        // First try to get merit scores if they exist
        var meritScores = await _admissionsRepository.GetMeritScoresByPercentageAsync(
            request.MinimumPercentage,
            cancellationToken);

        _logger.LogInformation(
            "Found {Count} merit scores with percentage >= {MinimumPercentage}%",
            meritScores.Count,
            request.MinimumPercentage);

        // If no merit scores exist, query application drafts directly
        List<ApplicantWithPercentage> applicantsWithPercentage = new();
        
        if (meritScores.Count == 0)
        {
            _logger.LogInformation(
                "No merit scores found. Querying application drafts directly for applicants with percentage >= {MinimumPercentage}%.",
                request.MinimumPercentage);

            // Get all submitted applications (payment not required for direct admission eligibility)
            // We'll query all accounts and filter by submitted status
            var allAccounts = await _accountRepository.GetAllAsync(cancellationToken);
            var submittedAccounts = allAccounts
                .Where(a => a.IsApplicationSubmitted)
                .ToList();

            foreach (var account in submittedAccounts)
            {
                // Skip if already enrolled
                if (account.Status == ApplicationStatus.Enrolled)
                {
                    continue;
                }

                // Get application draft
                var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(account.Id, cancellationToken);
                if (draftEntity == null)
                {
                    continue;
                }

                var draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _serializerOptions)
                           ?? ApplicantApplicationDraftDto.Empty;

                // Parse percentage from draft
                var classXIIPercentage = ParsePercentage(draft.Academics?.BoardExamination?.Percentage);
                
                if (classXIIPercentage >= request.MinimumPercentage)
                {
                    var shift = account.Shift ?? draft.Courses?.Shift ?? string.Empty;
                    var majorSubject = draft.Courses?.MajorSubject ?? string.Empty;

                    applicantsWithPercentage.Add(new ApplicantWithPercentage
                    {
                        AccountId = account.Id,
                        ApplicationNumber = account.UniqueId,
                        FullName = account.FullName,
                        Email = account.Email,
                        MobileNumber = account.MobileNumber,
                        ClassXIIPercentage = classXIIPercentage,
                        Shift = shift,
                        MajorSubject = majorSubject,
                        Account = account
                    });
                }
            }

            if (applicantsWithPercentage.Count == 0)
            {
                _logger.LogWarning("No applicants found with percentage >= {MinimumPercentage}%.", request.MinimumPercentage);
                return new CreateDirectAdmissionOffersResult(0, new List<Guid>(), new List<string> { $"No applicants found with percentage >= {request.MinimumPercentage}%. Make sure applicants have submitted their applications with Class XII percentage filled in." });
            }

            _logger.LogInformation(
                "Found {Count} applicants with percentage >= {MinimumPercentage}% from application drafts.",
                applicantsWithPercentage.Count,
                request.MinimumPercentage);
        }

        var defaultExpiryDate = request.ExpiryDate ?? DateTime.UtcNow.AddDays(30);
        var createdOfferIds = new List<Guid>();
        var errors = new List<string>();

        // Process merit scores if they exist
        if (meritScores.Count > 0)
        {
            foreach (var meritScore in meritScores)
            {
                try
                {
                    _logger.LogInformation(
                        "Processing merit score for {ApplicationNumber} ({FullName}) - Class XII: {Percentage}%, Rank: {Rank}",
                        meritScore.ApplicationNumber,
                        meritScore.FullName,
                        meritScore.ClassXIIPercentage,
                        meritScore.Rank);

                    // Check if offer already exists (any status)
                    var existingOffer = await _admissionsRepository.GetOfferByAccountIdAsync(meritScore.AccountId, cancellationToken);
                    if (existingOffer != null)
                    {
                        _logger.LogInformation(
                            "Account {AccountId} ({ApplicationNumber}) already has an offer with status {Status}. Skipping.",
                            meritScore.AccountId,
                            meritScore.ApplicationNumber,
                            existingOffer.Status);
                        // Skip if offer exists (regardless of status - to avoid duplicates)
                        continue;
                    }

                    // Get account details
                    var account = await _accountRepository.GetByIdAsync(meritScore.AccountId, cancellationToken);
                    if (account == null)
                    {
                        var errorMsg = $"Account not found for merit score ID {meritScore.Id} (Application: {meritScore.ApplicationNumber})";
                        errors.Add(errorMsg);
                        _logger.LogWarning(errorMsg);
                        continue;
                    }

                    _logger.LogInformation(
                        "Account found: {ApplicationNumber}, Status: {Status}, IsPaymentCompleted: {IsPaymentCompleted}",
                        account.UniqueId,
                        account.Status,
                        account.IsPaymentCompleted);

                    // Skip if already enrolled
                    if (account.Status == ApplicationStatus.Enrolled)
                    {
                        _logger.LogInformation(
                            "Skipping account {AccountId} ({ApplicationNumber}) - already enrolled.",
                            meritScore.AccountId,
                            account.UniqueId);
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

                    // Update account status to Approved only if not already Approved
                    if (account.Status != ApplicationStatus.Approved)
                    {
                        account.UpdateStatus(
                            ApplicationStatus.Approved,
                            request.CreatedBy ?? "System",
                            DateTime.UtcNow,
                            $"Direct admission offer created based on {meritScore.ClassXIIPercentage}% in Class XII. Admission fee: ₹{request.AdmissionFeeAmount}");

                        await _accountRepository.UpdateAsync(account, cancellationToken);
                    }
                    else
                    {
                        _logger.LogInformation(
                            "Account {AccountId} ({ApplicationNumber}) is already in Approved status. Skipping status update.",
                            account.Id,
                            account.UniqueId);
                    }

                    // Send notification with admission fee payment requirement
                    try
                    {
                        await _notificationService.SendAdmissionFeePaymentNotificationAsync(
                            account.FullName,
                            account.Email,
                            account.MobileNumber,
                            offer.ApplicationNumber,
                            meritScore.ClassXIIPercentage,
                            offer.Shift,
                            offer.MajorSubject,
                            request.AdmissionFeeAmount,
                            offer.OfferDate,
                            offer.ExpiryDate,
                            cancellationToken);

                        _logger.LogInformation(
                            "Admission fee payment notification sent to {Email} for application {ApplicationNumber}",
                            account.Email,
                            offer.ApplicationNumber);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Failed to send admission fee payment notification to {Email} for application {ApplicationNumber}",
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
        }
        else
        {
            // Process applicants from application drafts
            foreach (var applicant in applicantsWithPercentage)
            {
                try
                {
                    // Check if offer already exists
                    var existingOffer = await _admissionsRepository.GetOfferByAccountIdAsync(applicant.AccountId, cancellationToken);
                    if (existingOffer != null && existingOffer.Status == OfferStatus.Pending)
                    {
                        _logger.LogInformation(
                            "Skipping account {AccountId} - already has a pending offer.",
                            applicant.AccountId);
                        continue;
                    }

                    // Skip if already enrolled
                    if (applicant.Account.Status == ApplicationStatus.Enrolled)
                    {
                        _logger.LogInformation(
                            "Skipping account {AccountId} - already enrolled.",
                            applicant.AccountId);
                        continue;
                    }

                    // Create offer (rank will be 0 since we don't have merit scores)
                    var offer = new AdmissionOffer(
                        applicant.AccountId,
                        applicant.ApplicationNumber,
                        applicant.FullName,
                        0, // No rank available without merit scores
                        applicant.Shift,
                        applicant.MajorSubject,
                        defaultExpiryDate,
                        request.CreatedBy ?? "System",
                        null);

                    await _admissionsRepository.AddAdmissionOfferAsync(offer, cancellationToken);
                    createdOfferIds.Add(offer.Id);

                    // Update account status to Approved only if not already Approved
                    if (applicant.Account.Status != ApplicationStatus.Approved)
                    {
                        applicant.Account.UpdateStatus(
                            ApplicationStatus.Approved,
                            request.CreatedBy ?? "System",
                            DateTime.UtcNow,
                            $"Direct admission offer created based on {applicant.ClassXIIPercentage}% in Class XII. Admission fee: ₹{request.AdmissionFeeAmount}");

                        await _accountRepository.UpdateAsync(applicant.Account, cancellationToken);
                    }
                    else
                    {
                        _logger.LogInformation(
                            "Account {AccountId} ({ApplicationNumber}) is already in Approved status. Skipping status update.",
                            applicant.AccountId,
                            applicant.ApplicationNumber);
                    }

                    // Send notification with admission fee payment requirement
                    try
                    {
                        await _notificationService.SendAdmissionFeePaymentNotificationAsync(
                            applicant.FullName,
                            applicant.Email,
                            applicant.MobileNumber,
                            offer.ApplicationNumber,
                            applicant.ClassXIIPercentage,
                            offer.Shift,
                            offer.MajorSubject,
                            request.AdmissionFeeAmount,
                            offer.OfferDate,
                            offer.ExpiryDate,
                            cancellationToken);

                        _logger.LogInformation(
                            "Admission fee payment notification sent to {Email} for application {ApplicationNumber}",
                            applicant.Email,
                            offer.ApplicationNumber);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Failed to send admission fee payment notification to {Email} for application {ApplicationNumber}",
                            applicant.Email,
                            offer.ApplicationNumber);
                        // Don't fail the bulk operation if notification fails
                    }
                }
                catch (Exception ex)
                {
                    var errorMsg = $"Failed to create offer for applicant {applicant.ApplicationNumber}: {ex.Message}";
                    errors.Add(errorMsg);
                    _logger.LogError(ex, errorMsg);
                }
            }
        }

        _logger.LogInformation(
            "Direct admission offer creation complete. Created {Count} offers, {ErrorCount} errors.",
            createdOfferIds.Count,
            errors.Count);

        return new CreateDirectAdmissionOffersResult(createdOfferIds.Count, createdOfferIds, errors);
    }

    private decimal ParsePercentage(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0;
        }

        // Remove % sign if present and any whitespace
        var cleaned = value.Trim().Replace("%", "").Trim();
        if (decimal.TryParse(cleaned, out var result))
        {
            return Math.Clamp(result, 0, 100);
        }

        return 0;
    }

    private class ApplicantWithPercentage
    {
        public Guid AccountId { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public decimal ClassXIIPercentage { get; set; }
        public string Shift { get; set; } = string.Empty;
        public string MajorSubject { get; set; } = string.Empty;
        public StudentApplicantAccount Account { get; set; } = null!;
    }
}


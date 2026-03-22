using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.SendIndividualAdmissionOffer;

public sealed class SendIndividualAdmissionOfferCommandHandler : IRequestHandler<SendIndividualAdmissionOfferCommand, SendIndividualAdmissionOfferResult>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly IAdmissionsRepository _admissionsRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<SendIndividualAdmissionOfferCommandHandler> _logger;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public SendIndividualAdmissionOfferCommandHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository,
        IAdmissionsRepository admissionsRepository,
        IApplicantNotificationService notificationService,
        ILogger<SendIndividualAdmissionOfferCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
        _admissionsRepository = admissionsRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<SendIndividualAdmissionOfferResult> Handle(SendIndividualAdmissionOfferCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Sending individual admission offer to application {ApplicationNumber}. Fee: ₹{AdmissionFee}, Expiry: {ExpiryDays} days",
            request.ApplicationNumber,
            request.AdmissionFeeAmount,
            request.ExpiryDays);

        // Get account by application number
        var account = await _accountRepository.GetByUniqueIdAsync(request.ApplicationNumber, cancellationToken);
        if (account == null)
        {
            throw new InvalidOperationException($"Account with application number {request.ApplicationNumber} not found.");
        }

        // Check if already enrolled
        if (account.Status == ApplicationStatus.Enrolled)
        {
            throw new InvalidOperationException($"Account {request.ApplicationNumber} is already enrolled.");
        }

        // Check if offer already exists
        var existingOffer = await _admissionsRepository.GetOfferByAccountIdAsync(account.Id, cancellationToken);
        if (existingOffer != null && existingOffer.Status == OfferStatus.Pending)
        {
            throw new InvalidOperationException($"Account {request.ApplicationNumber} already has a pending offer.");
        }

        // Get application draft to extract shift and major subject
        var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(account.Id, cancellationToken);
        var draft = draftEntity != null
            ? JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _serializerOptions)
              ?? ApplicantApplicationDraftDto.Empty
            : ApplicantApplicationDraftDto.Empty;

        var shift = account.Shift ?? draft.Courses?.Shift ?? string.Empty;
        var majorSubject = draft.Courses?.MajorSubject ?? string.Empty;

        // Get merit score if available (for rank)
        var meritScore = await _admissionsRepository.GetMeritScoreByAccountIdAsync(account.Id, cancellationToken);
        var rank = meritScore?.Rank ?? 0;

        // Calculate expiry date
        var expiryDate = DateTime.UtcNow.AddDays(request.ExpiryDays);

        // Create offer
        var offer = new AdmissionOffer(
            account.Id,
            account.UniqueId,
            account.FullName,
            rank,
            shift,
            majorSubject,
            expiryDate,
            request.CreatedBy ?? "System",
            request.Remarks);

        await _admissionsRepository.AddAdmissionOfferAsync(offer, cancellationToken);
        _logger.LogInformation("Admission offer created: {OfferId} for application {ApplicationNumber}", offer.Id, offer.ApplicationNumber);

        // Update account status to Approved if not already approved
        if (account.Status != ApplicationStatus.Approved)
        {
            account.UpdateStatus(
                ApplicationStatus.Approved,
                request.CreatedBy ?? "System",
                DateTime.UtcNow,
                $"Admission offer sent. Admission fee: ₹{request.AdmissionFeeAmount}. Expires in {request.ExpiryDays} days.");

            await _accountRepository.UpdateAsync(account, cancellationToken);
            _logger.LogInformation("Account {ApplicationNumber} status updated to Approved", account.UniqueId);
        }

        // Get Class XII percentage for notification
        var classXIIPercentage = ParsePercentage(draft.Academics?.BoardExamination?.Percentage);

        // Send notification with payment instructions
        try
        {
            await _notificationService.SendAdmissionFeePaymentNotificationAsync(
                account.FullName,
                account.Email,
                account.MobileNumber,
                offer.ApplicationNumber,
                classXIIPercentage,
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
            // Don't fail the command if notification fails
        }

        return new SendIndividualAdmissionOfferResult(
            offer.Id,
            offer.ApplicationNumber,
            offer.FullName,
            account.Email,
            offer.OfferDate,
            offer.ExpiryDate,
            request.AdmissionFeeAmount);
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
}





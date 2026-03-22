using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.UpdateOnlineApplicationStatus;

public sealed class UpdateOnlineApplicationStatusCommandHandler : IRequestHandler<UpdateOnlineApplicationStatusCommand, OnlineApplicationStatusUpdatedResult>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly IAdmissionErpSyncService _admissionErpSyncService;
    private readonly ILogger<UpdateOnlineApplicationStatusCommandHandler> _logger;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public UpdateOnlineApplicationStatusCommandHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository,
        IApplicantNotificationService notificationService,
        IAdmissionErpSyncService admissionErpSyncService,
        ILogger<UpdateOnlineApplicationStatusCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
        _notificationService = notificationService;
        _admissionErpSyncService = admissionErpSyncService;
        _logger = logger;
    }

    public async Task<OnlineApplicationStatusUpdatedResult> Handle(UpdateOnlineApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdForUpdateAsync(request.AccountId, cancellationToken);

        if (account is null)
        {
            throw new InvalidOperationException($"Account with ID {request.AccountId} not found.");
        }

        var previousStatus = account.Status;

        account.UpdateStatus(
            request.Status,
            request.UpdatedBy,
            DateTime.UtcNow,
            request.Remarks,
            request.EntranceExam?.ScheduledOnUtc,
            request.EntranceExam?.Venue,
            request.EntranceExam?.Instructions);

        await _accountRepository.UpdateAsync(account, cancellationToken);

        // Send notification if requested
        if (request.NotifyApplicant)
        {
            try
            {
                _logger.LogInformation(
                    "Preparing to send status update notification. AccountId: {AccountId}, Status: {Status}, Email: {Email}",
                    request.AccountId,
                    request.Status,
                    account.Email);

                // Fetch major subject from application draft if available
                string? majorSubject = null;
                if (account.IsApplicationSubmitted)
                {
                    var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(request.AccountId, cancellationToken);
                    if (draftEntity is not null)
                    {
                        var draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _serializerOptions);
                        majorSubject = draft?.Courses?.MajorSubject;
                        _logger.LogInformation(
                            "Retrieved major subject from draft: {MajorSubject}",
                            majorSubject ?? "Not found");
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Application is submitted but no draft found for account {AccountId}",
                            request.AccountId);
                    }
                }

                await _notificationService.SendStatusUpdateNotificationAsync(
                    account.FullName,
                    account.Email,
                    account.MobileNumber,
                    account.UniqueId,
                    request.Status,
                    request.Remarks,
                    request.EntranceExam?.ScheduledOnUtc,
                    request.EntranceExam?.Venue,
                    request.EntranceExam?.Instructions,
                    majorSubject,
                    request.PaymentDeadlineUtc,
                    cancellationToken);
                
                _logger.LogInformation(
                    "Status update notification sent successfully to applicant {Email} for application {UniqueId}. Status: {Status}",
                    account.Email,
                    account.UniqueId,
                    request.Status);
            }
            catch (Exception ex)
            {
                // Log error but don't fail the status update
                _logger.LogError(
                    ex,
                    "Failed to send status update notification to applicant {Email} for application {UniqueId}. Error: {ErrorMessage}",
                    account.Email,
                    account.UniqueId,
                    ex.Message);
            }
        }
        else
        {
            _logger.LogInformation(
                "Notification skipped - NotifyApplicant is false for account {AccountId}",
                request.AccountId);
        }

        if (request.Status == ApplicationStatus.Approved && previousStatus != ApplicationStatus.Approved)
        {
            try
            {
                var sync = await _admissionErpSyncService.TrySyncApprovedApplicationAsync(request.AccountId, cancellationToken);
                if (sync.Success)
                {
                    _logger.LogInformation(
                        "Admission→ERP sync OK for {AccountId}. ErpStudentId={ErpStudentId}. {Message}",
                        request.AccountId,
                        sync.ErpStudentId,
                        sync.Message);
                }
                else
                {
                    _logger.LogWarning(
                        "Admission→ERP sync did not complete for {AccountId}: {Message}",
                        request.AccountId,
                        sync.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admission→ERP sync threw for account {AccountId}", request.AccountId);
            }
        }

        return new OnlineApplicationStatusUpdatedResult(request.AccountId);
    }
}





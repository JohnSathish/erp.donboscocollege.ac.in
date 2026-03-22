using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Helpers;
using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Queries.GetAdmissionFee;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ERP.Application.Admissions.Options;

namespace ERP.Application.Admissions.Commands.GrantDirectAdmission;

public sealed class GrantDirectAdmissionCommandHandler : IRequestHandler<GrantDirectAdmissionCommand, GrantDirectAdmissionResult>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly IAdmissionFeeService _admissionFeeService;
    private readonly IOptions<AdmissionsWorkflowOptions> _workflowOptions;
    private readonly IAdmissionWorkflowSettingsService _workflowSettings;
    private readonly ILogger<GrantDirectAdmissionCommandHandler> _logger;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public GrantDirectAdmissionCommandHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository,
        IApplicantNotificationService notificationService,
        IAdmissionFeeService admissionFeeService,
        IOptions<AdmissionsWorkflowOptions> workflowOptions,
        IAdmissionWorkflowSettingsService workflowSettings,
        ILogger<GrantDirectAdmissionCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
        _notificationService = notificationService;
        _admissionFeeService = admissionFeeService;
        _workflowOptions = workflowOptions;
        _workflowSettings = workflowSettings;
        _logger = logger;
    }

    public async Task<GrantDirectAdmissionResult> Handle(GrantDirectAdmissionCommand request, CancellationToken cancellationToken)
    {
        var cutoff = await _workflowSettings.GetMeritClassXiiCutoffPercentageAsync(cancellationToken);
        var baseUrl = _workflowOptions.Value.ApplicantPortalBaseUrl.TrimEnd('/');

        var account = await _accountRepository.GetByIdForUpdateAsync(request.AccountId, cancellationToken)
                         ?? throw new InvalidOperationException($"Account {request.AccountId} not found.");

        if (!account.IsApplicationSubmitted)
        {
            throw new InvalidOperationException("Application must be submitted before granting direct admission.");
        }

        if (account.Status is ApplicationStatus.Approved or ApplicationStatus.Enrolled or ApplicationStatus.DirectAdmissionGranted
            or ApplicationStatus.AdmissionFeePaid)
        {
            throw new InvalidOperationException(
                $"Cannot grant direct admission. Current status is {account.Status}.");
        }

        var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(account.Id, cancellationToken);
        if (draftEntity is null)
        {
            throw new InvalidOperationException("Application draft not found.");
        }

        var draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _serializerOptions)
                    ?? ApplicantApplicationDraftDto.Empty;

        var parsed = AcademicPercentageParser.ParseClassXiiPercentage(draft.Academics?.BoardExamination?.Percentage);
        var pct = account.ClassXIIPercentage ?? parsed;
        if (pct <= 0)
        {
            pct = parsed;
        }

        if (pct < cutoff)
        {
            throw new InvalidOperationException(
                $"Class XII percentage ({pct:F2}%) is below the direct admission cutoff ({cutoff:F2}%).");
        }

        account.SetClassXiPercentage(pct);

        var shift = string.IsNullOrWhiteSpace(account.Shift) ? draft.Courses?.Shift ?? "" : account.Shift;
        var major = draft.Courses?.MajorSubject ?? "";
        var fee = _admissionFeeService.GetAdmissionFee(string.IsNullOrWhiteSpace(major) ? null : major);

        var offerDate = DateTime.UtcNow;
        var expiry = offerDate.AddDays(30);

        account.UpdateStatus(
            ApplicationStatus.DirectAdmissionGranted,
            request.PerformedBy,
            DateTime.UtcNow,
            $"Direct admission granted. Class XII: {pct:F2}%. Admission fee: ₹{fee:N2}.");

        await _accountRepository.UpdateAsync(account, cancellationToken);

        var paymentUrl = $"{baseUrl}/pay-admission/{account.Id}";

        _logger.LogInformation(
            "AUDIT DirectAdmissionGranted accountId={AccountId} uniqueId={UniqueId} pct={Pct} by={By}",
            account.Id,
            account.UniqueId,
            pct,
            request.PerformedBy ?? "unknown");

        if (request.NotifyApplicant)
        {
            try
            {
                await _notificationService.SendAdmissionFeePaymentNotificationAsync(
                    account.FullName,
                    account.Email,
                    account.MobileNumber,
                    account.UniqueId,
                    pct,
                    shift,
                    major,
                    fee,
                    offerDate,
                    expiry,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send direct admission notifications for {AccountId}", account.Id);
            }
        }

        return new GrantDirectAdmissionResult(account.Id, paymentUrl, pct, ApplicationStatus.DirectAdmissionGranted.ToString());
    }
}

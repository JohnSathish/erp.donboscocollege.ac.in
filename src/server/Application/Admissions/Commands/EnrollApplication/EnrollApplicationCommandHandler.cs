using System.Text.Json;
using ERP.Application.Admissions.Commands.CreateStudentAndGuardiansFromOffer;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.EnrollApplication;

public sealed class EnrollApplicationCommandHandler : IRequestHandler<EnrollApplicationCommand, EnrollApplicationResult>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly IAdmissionFeeService _admissionFeeService;
    private readonly IMediator _mediator;
    private readonly ILogger<EnrollApplicationCommandHandler> _logger;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public EnrollApplicationCommandHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository,
        IApplicantNotificationService notificationService,
        IAdmissionFeeService admissionFeeService,
        IMediator mediator,
        ILogger<EnrollApplicationCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
        _notificationService = notificationService;
        _admissionFeeService = admissionFeeService;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<EnrollApplicationResult> Handle(EnrollApplicationCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdForUpdateAsync(request.AccountId, cancellationToken);

        if (account is null)
        {
            throw new InvalidOperationException($"Account with ID {request.AccountId} not found.");
        }

        // Get the major subject from application draft to determine admission fee
        string? majorSubject = null;
        if (account.IsApplicationSubmitted)
        {
            var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(request.AccountId, cancellationToken);
            if (draftEntity is not null)
            {
                var draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _serializerOptions);
                majorSubject = draft?.Courses?.MajorSubject;
            }
        }

        // Validate payment amount matches required admission fee
        var requiredFee = _admissionFeeService.GetAdmissionFee(majorSubject);
        if (!_admissionFeeService.IsPaymentAmountValid(account.PaymentAmount, majorSubject))
        {
            throw new InvalidOperationException(
                $"Cannot enroll application {account.UniqueId}. " +
                $"Payment amount (₹{account.PaymentAmount ?? 0:N2}) does not meet the required admission fee of ₹{requiredFee:N2} " +
                $"for {(string.IsNullOrWhiteSpace(majorSubject) ? "the selected stream" : $"stream: {majorSubject}")}.");
        }

        var enrolledOnUtc = DateTime.UtcNow;

        // Enroll the application
        account.Enroll(request.EnrolledBy, enrolledOnUtc, request.Remarks);

        await _accountRepository.UpdateAsync(account, cancellationToken);

        _logger.LogInformation(
            "Application {ApplicationNumber} (AccountId: {AccountId}) has been enrolled by {EnrolledBy}",
            account.UniqueId,
            request.AccountId,
            request.EnrolledBy ?? "System");

        // Automatically create student and guardians from enrolled applicant
        try
        {
            var academicYear = DateTime.UtcNow.Year.ToString(); // Default to current year, can be made configurable
            var createStudentCommand = new CreateStudentAndGuardiansFromOfferCommand(
                account.Id,
                academicYear,
                null, // ProgramId - can be determined from offer or application
                null, // ProgramCode - can be determined from application
                null, // StudentNumber - will be auto-generated
                request.EnrolledBy);

            var studentResult = await _mediator.Send(createStudentCommand, cancellationToken);

            _logger.LogInformation(
                "Automatically created student {StudentNumber} (StudentId: {StudentId}) with {GuardianCount} guardians for enrolled applicant {ApplicationNumber}",
                studentResult.StudentNumber,
                studentResult.StudentId,
                studentResult.GuardiansCreated,
                account.UniqueId);
        }
        catch (Exception ex)
        {
            // Log error but don't fail the enrollment
            _logger.LogError(
                ex,
                "Failed to automatically create student and guardians for enrolled applicant {ApplicationNumber}. Error: {ErrorMessage}",
                account.UniqueId,
                ex.Message);
        }

        // Send enrollment notification if requested
        if (request.NotifyApplicant)
        {
            try
            {
                await _notificationService.SendEnrollmentNotificationAsync(
                    account.FullName,
                    account.Email,
                    account.MobileNumber,
                    account.UniqueId,
                    enrolledOnUtc,
                    request.Remarks,
                    cancellationToken);

                _logger.LogInformation(
                    "Enrollment notification sent successfully to applicant {Email} for application {UniqueId}",
                    account.Email,
                    account.UniqueId);
            }
            catch (Exception ex)
            {
                // Log error but don't fail the enrollment
                _logger.LogError(
                    ex,
                    "Failed to send enrollment notification to applicant {Email} for application {UniqueId}. Error: {ErrorMessage}",
                    account.Email,
                    account.UniqueId,
                    ex.Message);
            }
        }

        return new EnrollApplicationResult(
            account.Id,
            account.UniqueId,
            account.FullName,
            enrolledOnUtc);
    }
}


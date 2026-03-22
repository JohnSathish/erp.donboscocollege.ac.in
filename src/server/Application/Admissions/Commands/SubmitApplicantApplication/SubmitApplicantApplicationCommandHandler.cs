using System.Text.Json;
using ERP.Application.Admissions;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.SubmitApplicantApplication;

public sealed class SubmitApplicantApplicationCommandHandler
    : IRequestHandler<SubmitApplicantApplicationCommand, ApplicantApplicationPdfResult>
{
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly IApplicantApplicationPdfService _pdfService;
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public SubmitApplicantApplicationCommandHandler(
        IApplicantApplicationRepository applicationRepository,
        IApplicantApplicationPdfService pdfService,
        IApplicantAccountRepository accountRepository,
        IApplicantNotificationService notificationService)
    {
        _applicationRepository = applicationRepository;
        _pdfService = pdfService;
        _accountRepository = accountRepository;
        _notificationService = notificationService;
    }

    public async Task<ApplicantApplicationPdfResult> Handle(
        SubmitApplicantApplicationCommand request,
        CancellationToken cancellationToken)
    {
        request.Payload.CoursesLocked = true;

        ApplicantCourseSubjectNormalizer.Normalize(request.Payload);
        AcademicSectionDraftSync.PushToLegacy(request.Payload.Academics);

        var serialized = JsonSerializer.Serialize(request.Payload, _serializerOptions);
        var draft = new ApplicantApplicationDraft(request.AccountId, serialized);
        draft.Update(serialized, DateTime.UtcNow);

        await _applicationRepository.UpsertDraftAsync(draft, cancellationToken);

        var account = await _accountRepository.GetByIdForUpdateAsync(request.AccountId, cancellationToken)
                      ?? throw new InvalidOperationException("Applicant account could not be found.");

        if (account.AdmissionChannel == AdmissionChannel.Offline)
        {
            var email = request.Payload.Address?.Email?.Trim();
            if (!string.IsNullOrWhiteSpace(email) && email.Contains('@', StringComparison.Ordinal))
            {
                if (await _accountRepository.EmailExistsForOtherAccountAsync(email, account.Id, cancellationToken))
                {
                    throw new InvalidOperationException("Another applicant is already using this email address.");
                }
            }

            var pi = request.Payload.PersonalInformation;
            var name = string.IsNullOrWhiteSpace(pi.NameAsPerAdmitCard)
                ? account.FullName
                : pi.NameAsPerAdmitCard.Trim();
            var dob = TryParseDob(pi.DateOfBirth, account.DateOfBirth);
            var gender = string.IsNullOrWhiteSpace(pi.Gender) ? account.Gender : pi.Gender.Trim();
            account.ApplySubmissionProfile(name, dob, gender, email);
        }

        // Mark application as submitted
        account.MarkApplicationSubmitted();

        var shift = request.Payload.Courses.Shift;
        if (!string.IsNullOrWhiteSpace(shift))
        {
            account.UpdateShift(shift);
        }

        await _accountRepository.UpdateAsync(account, cancellationToken);

        var pdf = await _pdfService.GenerateAsync(
            request.Payload,
            account.IsPaymentCompleted,
            account.PaymentAmount,
            account.PhotoUrl,
            account.PaymentTransactionId,
            applicationNumber: account.UniqueId,
            submittedOnUtc: DateTime.UtcNow,
            cancellationToken);
        await _notificationService.SendApplicationSubmissionNotificationAsync(
            account.FullName,
            account.Email,
            account.UniqueId,
            pdf.Content,
            pdf.FileName,
            cancellationToken);

        return pdf;
    }

    private static DateOnly TryParseDob(string? raw, DateOnly fallback)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return fallback;
        }

        if (DateOnly.TryParse(raw, out var d))
        {
            return d;
        }

        return DateTime.TryParse(raw, out var dt) ? DateOnly.FromDateTime(dt) : fallback;
    }
}


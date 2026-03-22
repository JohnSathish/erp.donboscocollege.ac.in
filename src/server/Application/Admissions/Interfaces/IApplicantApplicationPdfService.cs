using ERP.Application.Admissions.DTOs;

namespace ERP.Application.Admissions.Interfaces;

public interface IApplicantApplicationPdfService
{
    Task<ApplicantApplicationPdfResult> GenerateAsync(
        ApplicantApplicationDraftDto payload,
        bool isPaymentCompleted,
        decimal? paymentAmount,
        string? photoUrl = null,
        string? transactionId = null,
        string? applicationNumber = null,
        DateTime? submittedOnUtc = null,
        CancellationToken cancellationToken = default);
}






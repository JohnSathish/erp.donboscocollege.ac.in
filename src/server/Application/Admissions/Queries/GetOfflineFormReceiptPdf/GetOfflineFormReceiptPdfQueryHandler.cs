using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetOfflineFormReceiptPdf;

public sealed class GetOfflineFormReceiptPdfQueryHandler
    : IRequestHandler<GetOfflineFormReceiptPdfQuery, OfflineFormReceiptPdfQueryResult>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IOfflineFormIssuanceRepository _issuanceRepository;
    private readonly IOfflineFormReceiptPdfService _receiptPdf;

    public GetOfflineFormReceiptPdfQueryHandler(
        IApplicantAccountRepository accountRepository,
        IOfflineFormIssuanceRepository issuanceRepository,
        IOfflineFormReceiptPdfService receiptPdf)
    {
        _accountRepository = accountRepository;
        _issuanceRepository = issuanceRepository;
        _receiptPdf = receiptPdf;
    }

    public async Task<OfflineFormReceiptPdfQueryResult> Handle(
        GetOfflineFormReceiptPdfQuery request,
        CancellationToken cancellationToken)
    {
        var formNumber = NormalizeFormNumber(request.FormNumber);
        if (formNumber is null)
        {
            throw new InvalidOperationException("Form number must be exactly 6 digits.");
        }

        var account = await _accountRepository.GetByUniqueIdAsync(formNumber, cancellationToken);
        if (account is not null && account.AdmissionChannel == AdmissionChannel.Offline)
        {
            var major = account.OfflineIssuedMajorSubject ?? account.Shift;
            var issuedOn = account.PaymentCompletedOnUtc ?? account.CreatedOnUtc;
            var amount = account.PaymentAmount ?? 0m;

            var (content, fileName) = await _receiptPdf.GenerateAsync(
                formNumber,
                account.FullName,
                major,
                amount,
                issuedOn,
                account.MobileNumber,
                cancellationToken);

            return new OfflineFormReceiptPdfQueryResult(content, fileName);
        }

        var issuance = await _issuanceRepository.GetByFormNumberAsync(formNumber, cancellationToken);
        if (issuance is not null && issuance.ApplicantAccountId is null)
        {
            var (content, fileName) = await _receiptPdf.GenerateAsync(
                formNumber,
                issuance.StudentName,
                majorSubject: null,
                issuance.ApplicationFeeAmount,
                issuance.IssuedOnUtc,
                mobileNumberForReceipt: null,
                cancellationToken);

            return new OfflineFormReceiptPdfQueryResult(content, fileName);
        }

        throw new InvalidOperationException($"No application fee receipt found for form number {formNumber}.");
    }

    private static string? NormalizeFormNumber(string raw)
    {
        var s = raw.Trim();
        if (s.Length != 6 || !s.All(char.IsDigit))
        {
            return null;
        }

        return s;
    }
}

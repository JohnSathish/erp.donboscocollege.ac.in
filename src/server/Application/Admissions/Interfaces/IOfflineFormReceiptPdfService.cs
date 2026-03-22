namespace ERP.Application.Admissions.Interfaces;

public interface IOfflineFormReceiptPdfService
{
    /// <param name="majorSubject">Null on fee receipt at issuance (subject chosen at receive).</param>
    /// <param name="mobileNumberForReceipt">Null to omit mobile from the PDF (issuance receipt).</param>
    Task<(byte[] Content, string FileName)> GenerateAsync(
        string formNumber,
        string studentName,
        string? majorSubject,
        decimal amountPaid,
        DateTime issuedOnUtc,
        string? mobileNumberForReceipt,
        CancellationToken cancellationToken = default);
}

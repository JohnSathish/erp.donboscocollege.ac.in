using ERP.Application.Admissions.ViewModels;

namespace ERP.Application.Admissions.Interfaces;

public enum ApplicantExportFormat
{
    Csv,
    Excel,
    Pdf
}

public sealed record ApplicantExportResult(string FileName, string ContentType, byte[] Content);

public interface IApplicantExportService
{
    Task<ApplicantExportResult> ExportAsync(ApplicantExportFormat format, CancellationToken cancellationToken = default);
    
    Task<ApplicantExportResult> ExportPaidApplicationsWithFullDetailsAsync(CancellationToken cancellationToken = default);

    /// <summary>All submitted online applications with full form columns (paid or unpaid).</summary>
    Task<ApplicantExportResult> ExportSubmittedApplicationsWithFullDetailsAsync(CancellationToken cancellationToken = default);
}








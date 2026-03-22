namespace ERP.Application.Admissions.Interfaces;

/// <summary>Admission boundary → ERP student registry. Idempotent: safe to retry.</summary>
public interface IAdmissionErpSyncService
{
    /// <summary>Creates or links <see cref="ERP.Domain.Students.Entities.Student"/> when application is approved.</summary>
    Task<AdmissionErpSyncResult> TrySyncApprovedApplicationAsync(Guid applicantAccountId, CancellationToken cancellationToken = default);
}

public sealed record AdmissionErpSyncResult(
    bool Success,
    Guid? ErpStudentId,
    string Message);

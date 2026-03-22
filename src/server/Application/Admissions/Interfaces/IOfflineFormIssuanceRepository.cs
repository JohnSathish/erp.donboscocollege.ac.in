using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IOfflineFormIssuanceRepository
{
    Task AddAsync(OfflineFormIssuance issuance, CancellationToken cancellationToken = default);

    Task<OfflineFormIssuance?> GetByFormNumberAsync(string formNumber, CancellationToken cancellationToken = default);

    Task<OfflineFormIssuance?> GetByFormNumberForUpdateAsync(string formNumber, CancellationToken cancellationToken = default);

    Task<bool> FormNumberExistsAsync(string formNumber, CancellationToken cancellationToken = default);

    /// <summary>True if this mobile is already used by a pending issuance (no account yet).</summary>
    Task<bool> PendingMobileExistsAsync(string mobileNumber, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task UpdateAsync(OfflineFormIssuance issuance, CancellationToken cancellationToken = default);
}

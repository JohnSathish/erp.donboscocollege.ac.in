using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IApplicantApplicationRepository
{
    Task<ApplicantApplicationDraft?> GetDraftByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApplicantApplicationDraft>> GetDraftsByAccountIdsAsync(
        IReadOnlyCollection<Guid> accountIds,
        CancellationToken cancellationToken = default);

    Task UpsertDraftAsync(ApplicantApplicationDraft draft, CancellationToken cancellationToken = default);
}






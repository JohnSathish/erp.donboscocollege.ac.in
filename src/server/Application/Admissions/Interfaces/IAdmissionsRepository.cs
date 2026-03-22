using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IAdmissionsRepository
{
    Task<Applicant> AddApplicantAsync(Applicant applicant, CancellationToken cancellationToken = default);

    Task<Applicant?> GetApplicantByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task UpsertMeritScoresAsync(
        IReadOnlyCollection<MeritScore> meritScores,
        IReadOnlyCollection<Guid>? removeMeritScoresForAccountIds = null,
        CancellationToken cancellationToken = default);

    Task<MeritScore?> GetMeritScoreByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);

    Task AddAdmissionOfferAsync(AdmissionOffer offer, CancellationToken cancellationToken = default);

    Task<AdmissionOffer?> GetOfferByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);

    Task<AdmissionOffer?> GetOfferByIdAsync(Guid offerId, CancellationToken cancellationToken = default);

    Task UpdateAdmissionOfferAsync(AdmissionOffer offer, CancellationToken cancellationToken = default);

    Task<(IReadOnlyCollection<MeritScore> MeritScores, int TotalCount)> GetMeritScoresAsync(
        string? shift = null,
        string? majorSubject = null,
        int page = 1,
        int pageSize = 50,
        decimal? minimumClassXiiPercentage = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MeritScore>> GetMeritScoresForBulkOffersAsync(
        string? shift = null,
        string? majorSubject = null,
        int? topNRanks = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MeritScore>> GetMeritScoresByPercentageAsync(
        decimal minimumPercentage,
        CancellationToken cancellationToken = default);
}


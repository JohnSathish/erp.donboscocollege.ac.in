using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public class AdmissionsRepository : IAdmissionsRepository
{
    private readonly ApplicationDbContext _context;

    public AdmissionsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Applicant> AddApplicantAsync(Applicant applicant, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Applicants.AddAsync(applicant, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await _context.Entry(entry.Entity).ReloadAsync(cancellationToken);

        return entry.Entity;
    }

    public Task<Applicant?> GetApplicantByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Applicants.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpsertMeritScoresAsync(
        IReadOnlyCollection<MeritScore> meritScores,
        IReadOnlyCollection<Guid>? removeMeritScoresForAccountIds = null,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            if (removeMeritScoresForAccountIds is { Count: > 0 })
            {
                var toRemove = await _context.MeritScores
                    .Where(ms => removeMeritScoresForAccountIds.Contains(ms.AccountId))
                    .ToListAsync(cancellationToken);
                _context.MeritScores.RemoveRange(toRemove);
            }

            foreach (var meritScore in meritScores)
            {
                var existing = await _context.MeritScores
                    .FirstOrDefaultAsync(ms => ms.AccountId == meritScore.AccountId, cancellationToken);

                if (existing != null)
                {
                    _context.MeritScores.Remove(existing);
                    await _context.MeritScores.AddAsync(meritScore, cancellationToken);
                }
                else
                {
                    await _context.MeritScores.AddAsync(meritScore, cancellationToken);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public Task<MeritScore?> GetMeritScoreByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return _context.MeritScores
            .AsNoTracking()
            .FirstOrDefaultAsync(ms => ms.AccountId == accountId, cancellationToken);
    }

    public async Task AddAdmissionOfferAsync(AdmissionOffer offer, CancellationToken cancellationToken = default)
    {
        await _context.AdmissionOffers.AddAsync(offer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<AdmissionOffer?> GetOfferByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return _context.AdmissionOffers
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.AccountId == accountId, cancellationToken);
    }

    public Task<AdmissionOffer?> GetOfferByIdAsync(Guid offerId, CancellationToken cancellationToken = default)
    {
        return _context.AdmissionOffers
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == offerId, cancellationToken);
    }

    public async Task UpdateAdmissionOfferAsync(AdmissionOffer offer, CancellationToken cancellationToken = default)
    {
        _context.AdmissionOffers.Update(offer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyCollection<MeritScore> MeritScores, int TotalCount)> GetMeritScoresAsync(
        string? shift = null,
        string? majorSubject = null,
        int page = 1,
        int pageSize = 50,
        decimal? minimumClassXiiPercentage = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.MeritScores.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(shift))
        {
            query = query.Where(ms => ms.Shift == shift);
        }

        if (!string.IsNullOrWhiteSpace(majorSubject))
        {
            query = query.Where(ms => ms.MajorSubject == majorSubject);
        }

        if (minimumClassXiiPercentage.HasValue)
        {
            query = query.Where(ms => ms.ClassXIIPercentage >= minimumClassXiiPercentage.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var meritScores = await query
            .OrderBy(ms => ms.Rank)
            .ThenBy(ms => ms.Shift)
            .ThenBy(ms => ms.MajorSubject)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (meritScores, totalCount);
    }

    public async Task<IReadOnlyCollection<MeritScore>> GetMeritScoresForBulkOffersAsync(
        string? shift = null,
        string? majorSubject = null,
        int? topNRanks = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.MeritScores.AsQueryable();

        if (!string.IsNullOrWhiteSpace(shift))
        {
            query = query.Where(ms => ms.Shift == shift);
        }

        if (!string.IsNullOrWhiteSpace(majorSubject))
        {
            query = query.Where(ms => ms.MajorSubject == majorSubject);
        }

        // Order by rank ascending (rank 1 is best)
        query = query.OrderBy(ms => ms.Rank);

        // Apply top N filter if specified
        if (topNRanks.HasValue && topNRanks.Value > 0)
        {
            query = query.Take(topNRanks.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<MeritScore>> GetMeritScoresByPercentageAsync(
        decimal minimumPercentage,
        CancellationToken cancellationToken = default)
    {
        var query = _context.MeritScores
            .AsNoTracking()
            .Where(ms => ms.ClassXIIPercentage >= minimumPercentage)
            .OrderBy(ms => ms.Rank); // Order by rank ascending (rank 1 is best)

        return await query.ToListAsync(cancellationToken);
    }
}



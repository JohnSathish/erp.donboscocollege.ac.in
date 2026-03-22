using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public class ApplicantApplicationRepository : IApplicantApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public ApplicantApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<ApplicantApplicationDraft?> GetDraftByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return _context.ApplicantApplicationDrafts
            .FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken);
    }

    public async Task<IReadOnlyList<ApplicantApplicationDraft>> GetDraftsByAccountIdsAsync(
        IReadOnlyCollection<Guid> accountIds,
        CancellationToken cancellationToken = default)
    {
        if (accountIds.Count == 0)
        {
            return Array.Empty<ApplicantApplicationDraft>();
        }

        return await _context.ApplicantApplicationDrafts
            .AsNoTracking()
            .Where(x => accountIds.Contains(x.AccountId))
            .ToListAsync(cancellationToken);
    }

    public async Task UpsertDraftAsync(ApplicantApplicationDraft draft, CancellationToken cancellationToken = default)
    {
        var existing = await _context.ApplicantApplicationDrafts
            .FirstOrDefaultAsync(x => x.AccountId == draft.AccountId, cancellationToken);

        if (existing is null)
        {
            await _context.ApplicantApplicationDrafts.AddAsync(draft, cancellationToken);
        }
        else
        {
            existing.Update(draft.Data, draft.UpdatedOnUtc);
            _context.ApplicantApplicationDrafts.Update(existing);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}






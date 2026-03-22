using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public sealed class OfflineFormIssuanceRepository : IOfflineFormIssuanceRepository
{
    private readonly ApplicationDbContext _context;

    public OfflineFormIssuanceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OfflineFormIssuance issuance, CancellationToken cancellationToken = default)
    {
        await _context.OfflineFormIssuances.AddAsync(issuance, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<OfflineFormIssuance?> GetByFormNumberAsync(string formNumber, CancellationToken cancellationToken = default)
    {
        return _context.OfflineFormIssuances
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FormNumber == formNumber, cancellationToken);
    }

    public Task<OfflineFormIssuance?> GetByFormNumberForUpdateAsync(string formNumber, CancellationToken cancellationToken = default)
    {
        return _context.OfflineFormIssuances
            .FirstOrDefaultAsync(x => x.FormNumber == formNumber, cancellationToken);
    }

    public Task<bool> FormNumberExistsAsync(string formNumber, CancellationToken cancellationToken = default)
    {
        return _context.OfflineFormIssuances.AnyAsync(x => x.FormNumber == formNumber, cancellationToken);
    }

    public Task<bool> PendingMobileExistsAsync(string mobileNumber, CancellationToken cancellationToken = default)
    {
        return _context.OfflineFormIssuances.AnyAsync(
            x => x.MobileNumber == mobileNumber && x.ApplicantAccountId == null,
            cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _context.OfflineFormIssuances.CountAsync(cancellationToken);
    }

    public async Task UpdateAsync(OfflineFormIssuance issuance, CancellationToken cancellationToken = default)
    {
        _context.OfflineFormIssuances.Update(issuance);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

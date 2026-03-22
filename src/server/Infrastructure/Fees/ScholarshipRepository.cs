using ERP.Application.Fees.Interfaces;
using ERP.Domain.Fees.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Fees;

public class ScholarshipRepository : IScholarshipRepository
{
    private readonly ApplicationDbContext _context;

    public ScholarshipRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Scholarship> AddAsync(Scholarship scholarship, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Scholarships.AddAsync(scholarship, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public Task<Scholarship?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Scholarships
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Scholarship>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.Scholarships
            .AsNoTracking()
            .Where(s => s.StudentId == studentId)
            .OrderByDescending(s => s.EffectiveFrom)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Scholarship>> GetActiveScholarshipsByStudentIdAsync(Guid studentId, DateTime? asOfDate = null, CancellationToken cancellationToken = default)
    {
        var date = asOfDate ?? DateTime.UtcNow;
        return await _context.Scholarships
            .AsNoTracking()
            .Where(s => s.StudentId == studentId &&
                       s.IsActive &&
                       s.EffectiveFrom <= date &&
                       (s.EffectiveTo == null || s.EffectiveTo >= date))
            .OrderByDescending(s => s.EffectiveFrom)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Scholarship>> GetActiveScholarshipsAsync(DateTime? asOfDate = null, CancellationToken cancellationToken = default)
    {
        var date = asOfDate ?? DateTime.UtcNow;
        return await _context.Scholarships
            .AsNoTracking()
            .Where(s => s.IsActive &&
                       s.EffectiveFrom <= date &&
                       (s.EffectiveTo == null || s.EffectiveTo >= date))
            .OrderBy(s => s.StudentId)
            .ThenByDescending(s => s.EffectiveFrom)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Scholarship scholarship, CancellationToken cancellationToken = default)
    {
        _context.Scholarships.Update(scholarship);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}


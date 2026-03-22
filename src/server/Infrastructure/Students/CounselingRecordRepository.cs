using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Students;

public class CounselingRecordRepository : ICounselingRecordRepository
{
    private readonly ApplicationDbContext _context;

    public CounselingRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CounselingRecord> AddAsync(CounselingRecord record, CancellationToken cancellationToken = default)
    {
        var entry = await _context.CounselingRecords.AddAsync(record, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public Task<CounselingRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.CounselingRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<CounselingRecord>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.CounselingRecords
            .AsNoTracking()
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.SessionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CounselingRecord>> GetUpcomingSessionsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default)
    {
        var startDate = fromDate ?? DateTime.UtcNow;
        return await _context.CounselingRecords
            .AsNoTracking()
            .Where(r => r.SessionDate >= startDate && 
                       (r.Status == CounselingStatus.Scheduled || r.Status == CounselingStatus.InProgress))
            .OrderBy(r => r.SessionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CounselingRecord>> GetSessionsRequiringFollowUpAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CounselingRecords
            .AsNoTracking()
            .Where(r => r.IsFollowUpRequired && 
                       r.FollowUpDate.HasValue && 
                       r.FollowUpDate <= DateTime.UtcNow &&
                       r.Status != CounselingStatus.Completed)
            .OrderBy(r => r.FollowUpDate)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(CounselingRecord record, CancellationToken cancellationToken = default)
    {
        _context.CounselingRecords.Update(record);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}


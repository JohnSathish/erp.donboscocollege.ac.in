using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Students;

public class DisciplineRecordRepository : IDisciplineRecordRepository
{
    private readonly ApplicationDbContext _context;

    public DisciplineRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DisciplineRecord> AddAsync(DisciplineRecord record, CancellationToken cancellationToken = default)
    {
        var entry = await _context.DisciplineRecords.AddAsync(record, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public Task<DisciplineRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.DisciplineRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<DisciplineRecord>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.DisciplineRecords
            .AsNoTracking()
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.IncidentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DisciplineRecord>> GetUnresolvedRecordsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DisciplineRecords
            .AsNoTracking()
            .Where(r => !r.IsResolved)
            .OrderBy(r => r.IncidentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(DisciplineRecord record, CancellationToken cancellationToken = default)
    {
        _context.DisciplineRecords.Update(record);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}


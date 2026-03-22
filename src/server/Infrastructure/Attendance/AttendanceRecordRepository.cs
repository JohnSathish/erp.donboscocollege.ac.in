using ERP.Application.Attendance.Interfaces;
using ERP.Domain.Attendance.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Attendance;

public class AttendanceRecordRepository(ApplicationDbContext context) : IAttendanceRecordRepository
{
    public async Task<AttendanceRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.AttendanceRecords
            .Include(x => x.Session)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<AttendanceRecord?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.AttendanceRecords
            .Include(x => x.Session)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<AttendanceRecord>> GetBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await context.AttendanceRecords
            .Include(x => x.Session)
            .Where(x => x.SessionId == sessionId)
            .OrderBy(x => x.PersonId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AttendanceRecord>> GetByPersonIdAsync(Guid personId, PersonType personType, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = context.AttendanceRecords
            .Include(x => x.Session)
            .Where(x => x.PersonId == personId && x.PersonType == personType);

        if (fromDate.HasValue)
            query = query.Where(x => x.MarkedOnUtc >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(x => x.MarkedOnUtc <= toDate.Value);

        return await query
            .OrderByDescending(x => x.MarkedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<AttendanceRecord?> GetBySessionAndPersonAsync(Guid sessionId, Guid personId, CancellationToken cancellationToken = default)
    {
        return await context.AttendanceRecords
            .Include(x => x.Session)
            .FirstOrDefaultAsync(x => x.SessionId == sessionId && x.PersonId == personId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<AttendanceRecord>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, Guid? personId = null, PersonType? personType = null, CancellationToken cancellationToken = default)
    {
        var query = context.AttendanceRecords
            .Include(x => x.Session)
            .Where(x => x.MarkedOnUtc >= fromDate && x.MarkedOnUtc <= toDate);

        if (personId.HasValue)
            query = query.Where(x => x.PersonId == personId.Value);
        if (personType.HasValue)
            query = query.Where(x => x.PersonType == personType.Value);

        return await query
            .OrderByDescending(x => x.MarkedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<AttendanceRecord> AddAsync(AttendanceRecord record, CancellationToken cancellationToken = default)
    {
        await context.AttendanceRecords.AddAsync(record, cancellationToken);
        return record;
    }

    public async Task AddRangeAsync(IEnumerable<AttendanceRecord> records, CancellationToken cancellationToken = default)
    {
        await context.AttendanceRecords.AddRangeAsync(records, cancellationToken);
    }

    public Task UpdateAsync(AttendanceRecord record, CancellationToken cancellationToken = default)
    {
        context.AttendanceRecords.Update(record);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}


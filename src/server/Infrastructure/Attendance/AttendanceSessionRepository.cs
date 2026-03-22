using ERP.Application.Attendance.Interfaces;
using ERP.Domain.Attendance.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Attendance;

public class AttendanceSessionRepository(ApplicationDbContext context) : IAttendanceSessionRepository
{
    public async Task<AttendanceSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.AttendanceSessions
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<AttendanceSession?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.AttendanceSessions
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<AttendanceSession>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await context.AttendanceSessions
            .Where(x => x.SessionDate >= fromDate && x.SessionDate <= toDate)
            .OrderBy(x => x.SessionDate)
            .ThenBy(x => x.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AttendanceSession>> GetByClassSectionAsync(Guid classSectionId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = context.AttendanceSessions
            .Where(x => x.ClassSectionId == classSectionId);

        if (fromDate.HasValue)
            query = query.Where(x => x.SessionDate >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(x => x.SessionDate <= toDate.Value);

        return await query
            .OrderBy(x => x.SessionDate)
            .ThenBy(x => x.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AttendanceSession>> GetByCourseAsync(Guid courseId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = context.AttendanceSessions
            .Where(x => x.CourseId == courseId);

        if (fromDate.HasValue)
            query = query.Where(x => x.SessionDate >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(x => x.SessionDate <= toDate.Value);

        return await query
            .OrderBy(x => x.SessionDate)
            .ThenBy(x => x.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AttendanceSession>> GetByAcademicYearAsync(string academicYear, string? term = null, CancellationToken cancellationToken = default)
    {
        var query = context.AttendanceSessions
            .Where(x => x.AcademicYear == academicYear);

        if (!string.IsNullOrWhiteSpace(term))
            query = query.Where(x => x.Term == term);

        return await query
            .OrderBy(x => x.SessionDate)
            .ThenBy(x => x.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<AttendanceSession> AddAsync(AttendanceSession session, CancellationToken cancellationToken = default)
    {
        await context.AttendanceSessions.AddAsync(session, cancellationToken);
        return session;
    }

    public Task UpdateAsync(AttendanceSession session, CancellationToken cancellationToken = default)
    {
        context.AttendanceSessions.Update(session);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}


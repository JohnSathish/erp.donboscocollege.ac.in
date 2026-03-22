using ERP.Application.Attendance.Interfaces;
using ERP.Domain.Attendance.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Attendance;

public class AttendanceDeviceEventRepository(ApplicationDbContext context) : IAttendanceDeviceEventRepository
{
    public async Task<AttendanceDeviceEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.AttendanceDeviceEvents
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<AttendanceDeviceEvent>> GetUnprocessedEventsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default)
    {
        var query = context.AttendanceDeviceEvents
            .Where(x => !x.IsProcessed);

        if (fromDate.HasValue)
            query = query.Where(x => x.EventTimestamp >= fromDate.Value);

        return await query
            .OrderBy(x => x.EventTimestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AttendanceDeviceEvent>> GetByDeviceIdAsync(string deviceId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = context.AttendanceDeviceEvents
            .Where(x => x.DeviceId == deviceId);

        if (fromDate.HasValue)
            query = query.Where(x => x.EventTimestamp >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(x => x.EventTimestamp <= toDate.Value);

        return await query
            .OrderByDescending(x => x.EventTimestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AttendanceDeviceEvent>> GetByCardNumberAsync(string cardNumber, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = context.AttendanceDeviceEvents
            .Where(x => x.CardNumber == cardNumber);

        if (fromDate.HasValue)
            query = query.Where(x => x.EventTimestamp >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(x => x.EventTimestamp <= toDate.Value);

        return await query
            .OrderByDescending(x => x.EventTimestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<AttendanceDeviceEvent> AddAsync(AttendanceDeviceEvent deviceEvent, CancellationToken cancellationToken = default)
    {
        await context.AttendanceDeviceEvents.AddAsync(deviceEvent, cancellationToken);
        return deviceEvent;
    }

    public async Task AddRangeAsync(IEnumerable<AttendanceDeviceEvent> deviceEvents, CancellationToken cancellationToken = default)
    {
        await context.AttendanceDeviceEvents.AddRangeAsync(deviceEvents, cancellationToken);
    }

    public Task UpdateAsync(AttendanceDeviceEvent deviceEvent, CancellationToken cancellationToken = default)
    {
        context.AttendanceDeviceEvents.Update(deviceEvent);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}


using ERP.Domain.Attendance.Entities;

namespace ERP.Application.Attendance.Interfaces;

public interface IAttendanceDeviceEventRepository
{
    Task<AttendanceDeviceEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AttendanceDeviceEvent>> GetUnprocessedEventsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AttendanceDeviceEvent>> GetByDeviceIdAsync(string deviceId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AttendanceDeviceEvent>> GetByCardNumberAsync(string cardNumber, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<AttendanceDeviceEvent> AddAsync(AttendanceDeviceEvent deviceEvent, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<AttendanceDeviceEvent> deviceEvents, CancellationToken cancellationToken = default);
    Task UpdateAsync(AttendanceDeviceEvent deviceEvent, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


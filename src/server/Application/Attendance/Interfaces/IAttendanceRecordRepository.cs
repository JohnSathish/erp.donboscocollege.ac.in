using ERP.Domain.Attendance.Entities;

namespace ERP.Application.Attendance.Interfaces;

public interface IAttendanceRecordRepository
{
    Task<AttendanceRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AttendanceRecord?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AttendanceRecord>> GetBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AttendanceRecord>> GetByPersonIdAsync(Guid personId, PersonType personType, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<AttendanceRecord?> GetBySessionAndPersonAsync(Guid sessionId, Guid personId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AttendanceRecord>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, Guid? personId = null, PersonType? personType = null, CancellationToken cancellationToken = default);
    Task<AttendanceRecord> AddAsync(AttendanceRecord record, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<AttendanceRecord> records, CancellationToken cancellationToken = default);
    Task UpdateAsync(AttendanceRecord record, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


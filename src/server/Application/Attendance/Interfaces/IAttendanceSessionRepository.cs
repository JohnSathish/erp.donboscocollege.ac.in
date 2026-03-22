using ERP.Domain.Attendance.Entities;

namespace ERP.Application.Attendance.Interfaces;

public interface IAttendanceSessionRepository
{
    Task<AttendanceSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AttendanceSession?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AttendanceSession>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AttendanceSession>> GetByClassSectionAsync(Guid classSectionId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AttendanceSession>> GetByCourseAsync(Guid courseId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AttendanceSession>> GetByAcademicYearAsync(string academicYear, string? term = null, CancellationToken cancellationToken = default);
    Task<AttendanceSession> AddAsync(AttendanceSession session, CancellationToken cancellationToken = default);
    Task UpdateAsync(AttendanceSession session, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


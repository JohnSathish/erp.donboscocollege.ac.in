using ERP.Domain.Attendance.Entities;

namespace ERP.Application.Attendance.Interfaces;

public interface IAttendanceService
{
    Task<Guid> CreateSessionAsync(
        string sessionName,
        SessionType type,
        DateTime sessionDate,
        string academicYear,
        Guid? classSectionId = null,
        Guid? courseId = null,
        Guid? staffShiftId = null,
        TimeOnly? startTime = null,
        TimeOnly? endTime = null,
        string? term = null,
        string? remarks = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default);

    Task MarkAttendanceAsync(
        Guid sessionId,
        Guid personId,
        PersonType personType,
        AttendanceStatus status,
        string? markedBy = null,
        string? deviceId = null,
        string? deviceType = null,
        string? remarks = null,
        CancellationToken cancellationToken = default);

    Task MarkBulkAttendanceAsync(
        Guid sessionId,
        IReadOnlyCollection<BulkAttendanceItem> items,
        string? markedBy = null,
        CancellationToken cancellationToken = default);

    Task ProcessDeviceEventAsync(
        Guid deviceEventId,
        Guid sessionId,
        CancellationToken cancellationToken = default);

    Task<AttendanceSummaryDto> GetStudentAttendanceSummaryAsync(
        Guid studentId,
        string academicYear,
        string? term = null,
        CancellationToken cancellationToken = default);

    Task<AttendanceSummaryDto> GetStaffAttendanceSummaryAsync(
        Guid staffId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<AbsenteeAlertDto>> GetAbsenteesAsync(
        DateTime date,
        Guid? classSectionId = null,
        Guid? courseId = null,
        CancellationToken cancellationToken = default);
}

public sealed record BulkAttendanceItem(
    Guid PersonId,
    AttendanceStatus Status,
    string? Remarks = null);

public sealed record AttendanceSummaryDto(
    Guid PersonId,
    PersonType PersonType,
    int TotalSessions,
    int PresentCount,
    int AbsentCount,
    int LateCount,
    int ExcusedCount,
    int HalfDayCount,
    decimal AttendancePercentage,
    DateTime? FromDate,
    DateTime? ToDate);

public sealed record AbsenteeAlertDto(
    Guid PersonId,
    PersonType PersonType,
    string PersonName,
    string? PersonNumber,
    Guid SessionId,
    string SessionName,
    DateTime SessionDate,
    AttendanceStatus Status,
    string? ContactInfo);


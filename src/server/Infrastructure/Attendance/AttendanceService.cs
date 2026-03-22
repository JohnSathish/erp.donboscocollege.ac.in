using ERP.Application.Attendance.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Attendance.Entities;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Attendance;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceSessionRepository _sessionRepository;
    private readonly IAttendanceRecordRepository _recordRepository;
    private readonly IAttendanceDeviceEventRepository _deviceEventRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<AttendanceService> _logger;

    public AttendanceService(
        IAttendanceSessionRepository sessionRepository,
        IAttendanceRecordRepository recordRepository,
        IAttendanceDeviceEventRepository deviceEventRepository,
        IStudentRepository studentRepository,
        ILogger<AttendanceService> logger)
    {
        _sessionRepository = sessionRepository;
        _recordRepository = recordRepository;
        _deviceEventRepository = deviceEventRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<Guid> CreateSessionAsync(
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
        CancellationToken cancellationToken = default)
    {
        var session = new AttendanceSession(
            sessionName,
            type,
            sessionDate,
            academicYear,
            classSectionId,
            courseId,
            staffShiftId,
            startTime,
            endTime,
            term,
            remarks,
            createdBy);

        await _sessionRepository.AddAsync(session, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created attendance session {SessionName} (ID: {SessionId}) for {SessionDate}.",
            sessionName, session.Id, sessionDate);

        return session.Id;
    }

    public async Task MarkAttendanceAsync(
        Guid sessionId,
        Guid personId,
        PersonType personType,
        AttendanceStatus status,
        string? markedBy = null,
        string? deviceId = null,
        string? deviceType = null,
        string? remarks = null,
        CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdForUpdateAsync(sessionId, cancellationToken);
        if (session == null)
            throw new InvalidOperationException($"Attendance session {sessionId} not found.");

        var existingRecord = await _recordRepository.GetBySessionAndPersonAsync(sessionId, personId, cancellationToken);
        if (existingRecord != null)
        {
            existingRecord.UpdateStatus(status, markedBy, remarks);
            await _recordRepository.UpdateAsync(existingRecord, cancellationToken);
        }
        else
        {
            var record = new AttendanceRecord(
                sessionId,
                personId,
                personType,
                status,
                markedBy,
                deviceId,
                deviceType,
                remarks);

            await _recordRepository.AddAsync(record, cancellationToken);
        }

        await _recordRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Marked attendance for person {PersonId} in session {SessionId} as {Status}.",
            personId, sessionId, status);
    }

    public async Task MarkBulkAttendanceAsync(
        Guid sessionId,
        IReadOnlyCollection<BulkAttendanceItem> items,
        string? markedBy = null,
        CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdForUpdateAsync(sessionId, cancellationToken);
        if (session == null)
            throw new InvalidOperationException($"Attendance session {sessionId} not found.");

        var records = new List<AttendanceRecord>();
        var existingRecords = (await _recordRepository.GetBySessionIdAsync(sessionId, cancellationToken))
            .ToDictionary(r => r.PersonId);

        foreach (var item in items)
        {
            if (existingRecords.TryGetValue(item.PersonId, out var existing))
            {
                existing.UpdateStatus(item.Status, markedBy, item.Remarks);
                await _recordRepository.UpdateAsync(existing, cancellationToken);
            }
            else
            {
                // Determine person type from context (for now, assume Student)
                var record = new AttendanceRecord(
                    sessionId,
                    item.PersonId,
                    PersonType.Student, // TODO: Determine from context
                    item.Status,
                    markedBy,
                    remarks: item.Remarks);
                records.Add(record);
            }
        }

        if (records.Any())
        {
            await _recordRepository.AddRangeAsync(records, cancellationToken);
        }

        await _recordRepository.SaveChangesAsync(cancellationToken);
        session.MarkAsCompleted(markedBy ?? "System");
        await _sessionRepository.UpdateAsync(session, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Marked bulk attendance for {Count} persons in session {SessionId}.",
            items.Count, sessionId);
    }

    public async Task ProcessDeviceEventAsync(
        Guid deviceEventId,
        Guid sessionId,
        CancellationToken cancellationToken = default)
    {
        var deviceEvent = await _deviceEventRepository.GetByIdAsync(deviceEventId, cancellationToken);
        if (deviceEvent == null)
            throw new InvalidOperationException($"Device event {deviceEventId} not found.");

        if (deviceEvent.IsProcessed)
            return;

        if (!deviceEvent.PersonId.HasValue)
        {
            deviceEvent.MarkAsFailed("Person ID not found in device event.");
            await _deviceEventRepository.UpdateAsync(deviceEvent, cancellationToken);
            await _deviceEventRepository.SaveChangesAsync(cancellationToken);
            return;
        }

        try
        {
            var record = new AttendanceRecord(
                sessionId,
                deviceEvent.PersonId.Value,
                deviceEvent.PersonType ?? PersonType.Student,
                AttendanceStatus.Present,
                markedBy: "Device",
                deviceEvent.DeviceId,
                deviceEvent.DeviceType);

            await _recordRepository.AddAsync(record, cancellationToken);
            await _recordRepository.SaveChangesAsync(cancellationToken);

            deviceEvent.MarkAsProcessed(record.Id);
            await _deviceEventRepository.UpdateAsync(deviceEvent, cancellationToken);
            await _deviceEventRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Processed device event {DeviceEventId} and created attendance record {RecordId}.",
                deviceEventId, record.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing device event {DeviceEventId}.", deviceEventId);
            deviceEvent.MarkAsFailed(ex.Message);
            await _deviceEventRepository.UpdateAsync(deviceEvent, cancellationToken);
            await _deviceEventRepository.SaveChangesAsync(cancellationToken);
            throw;
        }
    }

    public async Task<AttendanceSummaryDto> GetStudentAttendanceSummaryAsync(
        Guid studentId,
        string academicYear,
        string? term = null,
        CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetByAcademicYearAsync(academicYear, term, cancellationToken);
        var sessionIds = sessions.Select(s => s.Id).ToList();

        if (!sessionIds.Any())
        {
            return new AttendanceSummaryDto(
                studentId,
                PersonType.Student,
                0, 0, 0, 0, 0, 0,
                0m,
                null,
                null);
        }

        var records = await _recordRepository.GetByPersonIdAsync(
            studentId,
            PersonType.Student,
            cancellationToken: cancellationToken);

        var relevantRecords = records.Where(r => sessionIds.Contains(r.SessionId)).ToList();

        var totalSessions = sessions.Count;
        var presentCount = relevantRecords.Count(r => r.Status == AttendanceStatus.Present);
        var absentCount = relevantRecords.Count(r => r.Status == AttendanceStatus.Absent);
        var lateCount = relevantRecords.Count(r => r.Status == AttendanceStatus.Late);
        var excusedCount = relevantRecords.Count(r => r.Status == AttendanceStatus.Excused);
        var halfDayCount = relevantRecords.Count(r => r.Status == AttendanceStatus.HalfDay);

        var attendancePercentage = totalSessions > 0
            ? (decimal)presentCount / totalSessions * 100
            : 0m;

        var fromDate = sessions.Min(s => s.SessionDate);
        var toDate = sessions.Max(s => s.SessionDate);

        return new AttendanceSummaryDto(
            studentId,
            PersonType.Student,
            totalSessions,
            presentCount,
            absentCount,
            lateCount,
            excusedCount,
            halfDayCount,
            attendancePercentage,
            fromDate,
            toDate);
    }

    public async Task<AttendanceSummaryDto> GetStaffAttendanceSummaryAsync(
        Guid staffId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default)
    {
        var records = await _recordRepository.GetByPersonIdAsync(
            staffId,
            PersonType.Staff,
            fromDate,
            toDate,
            cancellationToken);

        var totalSessions = records.Count;
        var presentCount = records.Count(r => r.Status == AttendanceStatus.Present);
        var absentCount = records.Count(r => r.Status == AttendanceStatus.Absent);
        var lateCount = records.Count(r => r.Status == AttendanceStatus.Late);
        var excusedCount = records.Count(r => r.Status == AttendanceStatus.Excused);
        var halfDayCount = records.Count(r => r.Status == AttendanceStatus.HalfDay);

        var attendancePercentage = totalSessions > 0
            ? (decimal)presentCount / totalSessions * 100
            : 0m;

        return new AttendanceSummaryDto(
            staffId,
            PersonType.Staff,
            totalSessions,
            presentCount,
            absentCount,
            lateCount,
            excusedCount,
            halfDayCount,
            attendancePercentage,
            fromDate,
            toDate);
    }

    public async Task<IReadOnlyCollection<AbsenteeAlertDto>> GetAbsenteesAsync(
        DateTime date,
        Guid? classSectionId = null,
        Guid? courseId = null,
        CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetByDateRangeAsync(date, date, cancellationToken);
        
        if (classSectionId.HasValue)
            sessions = sessions.Where(s => s.ClassSectionId == classSectionId).ToList();
        if (courseId.HasValue)
            sessions = sessions.Where(s => s.CourseId == courseId).ToList();

        var absentees = new List<AbsenteeAlertDto>();

        foreach (var session in sessions)
        {
            var records = await _recordRepository.GetBySessionIdAsync(session.Id, cancellationToken);
            var absentRecords = records.Where(r => r.Status == AttendanceStatus.Absent).ToList();

            foreach (var record in absentRecords)
            {
                if (record.PersonType == PersonType.Student)
                {
                    var student = await _studentRepository.GetByIdAsync(record.PersonId, cancellationToken);
                    if (student != null)
                    {
                        absentees.Add(new AbsenteeAlertDto(
                            record.PersonId,
                            PersonType.Student,
                            student.FullName,
                            student.StudentNumber,
                            session.Id,
                            session.SessionName,
                            session.SessionDate,
                            record.Status,
                            student.Email ?? student.MobileNumber));
                    }
                }
                // TODO: Add staff support when Staff entity is available
            }
        }

        return absentees;
    }
}


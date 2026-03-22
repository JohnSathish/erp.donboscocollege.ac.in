namespace ERP.Domain.Attendance.Entities;

public class AttendanceRecord
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid SessionId { get; private set; }
    public AttendanceSession Session { get; private set; } = null!;
    public Guid PersonId { get; private set; } // StudentId or StaffId
    public PersonType PersonType { get; private set; }
    public AttendanceStatus Status { get; private set; } = AttendanceStatus.Present;
    public DateTime MarkedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? MarkedBy { get; private set; }
    public string? DeviceId { get; private set; } // For biometric/RFID device tracking
    public string? DeviceType { get; private set; } // e.g., "Biometric", "RFID", "Manual"
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private AttendanceRecord() { }

    public AttendanceRecord(
        Guid sessionId,
        Guid personId,
        PersonType personType,
        AttendanceStatus status,
        string? markedBy = null,
        string? deviceId = null,
        string? deviceType = null,
        string? remarks = null,
        string? createdBy = null)
    {
        if (sessionId == Guid.Empty)
            throw new ArgumentException("Session ID is required.", nameof(sessionId));
        if (personId == Guid.Empty)
            throw new ArgumentException("Person ID is required.", nameof(personId));

        SessionId = sessionId;
        PersonId = personId;
        PersonType = personType;
        Status = status;
        MarkedBy = string.IsNullOrWhiteSpace(markedBy) ? null : markedBy.Trim();
        DeviceId = string.IsNullOrWhiteSpace(deviceId) ? null : deviceId.Trim();
        DeviceType = string.IsNullOrWhiteSpace(deviceType) ? null : deviceType.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
        MarkedOnUtc = DateTime.UtcNow;
    }

    public void UpdateStatus(AttendanceStatus status, string? updatedBy = null, string? remarks = null)
    {
        Status = status;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? Remarks : remarks.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateDeviceInfo(string deviceId, string deviceType, string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException("Device ID is required.", nameof(deviceId));
        if (string.IsNullOrWhiteSpace(deviceType))
            throw new ArgumentException("Device type is required.", nameof(deviceType));

        DeviceId = deviceId.Trim();
        DeviceType = deviceType.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum PersonType
{
    Student = 0,
    Staff = 1
}

public enum AttendanceStatus
{
    Present = 0,
    Absent = 1,
    Late = 2,
    Excused = 3,
    HalfDay = 4
}


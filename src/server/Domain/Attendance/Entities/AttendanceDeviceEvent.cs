namespace ERP.Domain.Attendance.Entities;

public class AttendanceDeviceEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string DeviceId { get; private set; } = string.Empty;
    public string DeviceType { get; private set; } = string.Empty; // e.g., "Biometric", "RFID"
    public Guid? PersonId { get; private set; } // StudentId or StaffId
    public PersonType? PersonType { get; private set; }
    public string? CardNumber { get; private set; } // RFID card number or biometric ID
    public DateTime EventTimestamp { get; private set; } = DateTime.UtcNow;
    public EventType EventType { get; private set; } = EventType.Scan;
    public bool IsProcessed { get; private set; } = false;
    public Guid? AttendanceRecordId { get; private set; } // Linked attendance record if processed
    public DateTime? ProcessedOnUtc { get; private set; }
    public string? ProcessingError { get; private set; }
    public string? RawData { get; private set; } // Raw device data for debugging
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    private AttendanceDeviceEvent() { }

    public AttendanceDeviceEvent(
        string deviceId,
        string deviceType,
        DateTime eventTimestamp,
        EventType eventType,
        string? cardNumber = null,
        Guid? personId = null,
        PersonType? personType = null,
        string? rawData = null)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException("Device ID is required.", nameof(deviceId));
        if (string.IsNullOrWhiteSpace(deviceType))
            throw new ArgumentException("Device type is required.", nameof(deviceType));

        DeviceId = deviceId.Trim();
        DeviceType = deviceType.Trim();
        EventTimestamp = eventTimestamp;
        EventType = eventType;
        CardNumber = string.IsNullOrWhiteSpace(cardNumber) ? null : cardNumber.Trim();
        PersonId = personId;
        PersonType = personType;
        RawData = string.IsNullOrWhiteSpace(rawData) ? null : rawData.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsProcessed(Guid attendanceRecordId)
    {
        IsProcessed = true;
        AttendanceRecordId = attendanceRecordId;
        ProcessedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsFailed(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("Error message is required.", nameof(errorMessage));

        ProcessingError = errorMessage.Trim();
        ProcessedOnUtc = DateTime.UtcNow;
    }
}

public enum EventType
{
    Scan = 0, // Card/biometric scan
    Entry = 1, // Entry event
    Exit = 2, // Exit event
    Error = 3 // Device error
}


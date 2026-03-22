namespace ERP.Domain.Attendance.Entities;

public class AttendanceSession
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string SessionName { get; private set; } = string.Empty;
    public SessionType Type { get; private set; }
    public Guid? ClassSectionId { get; private set; } // For class-based attendance
    public Guid? CourseId { get; private set; } // For course-based attendance
    public Guid? StaffShiftId { get; private set; } // For staff attendance
    public DateTime SessionDate { get; private set; }
    public TimeOnly? StartTime { get; private set; }
    public TimeOnly? EndTime { get; private set; }
    public string AcademicYear { get; private set; } = string.Empty;
    public string? Term { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsMarked { get; private set; } = false;
    public DateTime? MarkedOnUtc { get; private set; }
    public string? MarkedBy { get; private set; }
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private AttendanceSession() { }

    public AttendanceSession(
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
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(sessionName))
            throw new ArgumentException("Session name is required.", nameof(sessionName));
        if (string.IsNullOrWhiteSpace(academicYear))
            throw new ArgumentException("Academic year is required.", nameof(academicYear));

        SessionName = sessionName.Trim();
        Type = type;
        SessionDate = sessionDate;
        AcademicYear = academicYear.Trim();
        ClassSectionId = classSectionId;
        CourseId = courseId;
        StaffShiftId = staffShiftId;
        StartTime = startTime;
        EndTime = endTime;
        Term = string.IsNullOrWhiteSpace(term) ? null : term.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsCompleted(string markedBy, string? remarks = null)
    {
        IsMarked = true;
        MarkedOnUtc = DateTime.UtcNow;
        MarkedBy = string.IsNullOrWhiteSpace(markedBy) ? null : markedBy.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? Remarks : remarks.Trim();
        UpdatedBy = markedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Deactivate(string? updatedBy = null)
    {
        IsActive = false;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Activate(string? updatedBy = null)
    {
        IsActive = true;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum SessionType
{
    Daily = 0, // Full day attendance
    Period = 1, // Period/class-wise attendance
    StaffShift = 2, // Staff shift attendance
    Event = 3 // Special event attendance
}


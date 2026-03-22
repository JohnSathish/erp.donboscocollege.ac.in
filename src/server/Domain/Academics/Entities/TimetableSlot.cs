namespace ERP.Domain.Academics.Entities;

public class TimetableSlot
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ClassSectionId { get; private set; }
    public ClassSection ClassSection { get; private set; } = null!;
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public string? RoomNumber { get; private set; }
    public string? Building { get; private set; }
    public Guid? TeacherId { get; private set; }
    public string? TeacherName { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private TimetableSlot() { }

    public TimetableSlot(
        Guid classSectionId,
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        Guid? teacherId = null,
        string? teacherName = null,
        string? roomNumber = null,
        string? building = null,
        string? remarks = null,
        string? createdBy = null)
    {
        if (classSectionId == Guid.Empty)
            throw new ArgumentException("Class section ID is required.", nameof(classSectionId));
        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time.");

        ClassSectionId = classSectionId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        TeacherId = teacherId;
        TeacherName = string.IsNullOrWhiteSpace(teacherName) ? null : teacherName.Trim();
        RoomNumber = string.IsNullOrWhiteSpace(roomNumber) ? null : roomNumber.Trim();
        Building = string.IsNullOrWhiteSpace(building) ? null : building.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateTime(TimeOnly startTime, TimeOnly endTime, string? updatedBy = null)
    {
        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time.");

        StartTime = startTime;
        EndTime = endTime;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateRoom(string? roomNumber, string? building = null, string? updatedBy = null)
    {
        RoomNumber = string.IsNullOrWhiteSpace(roomNumber) ? null : roomNumber.Trim();
        Building = string.IsNullOrWhiteSpace(building) ? null : building.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void AssignTeacher(Guid teacherId, string? teacherName = null, string? updatedBy = null)
    {
        TeacherId = teacherId;
        TeacherName = string.IsNullOrWhiteSpace(teacherName) ? null : teacherName.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Deactivate(string? updatedBy = null)
    {
        IsActive = false;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}


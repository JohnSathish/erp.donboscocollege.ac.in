namespace ERP.Domain.Academics.Entities;

public class ClassSection
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string SectionName { get; private set; } = string.Empty; // e.g., "A", "B", "Morning", "Evening"
    public Guid CourseId { get; private set; }
    public Guid? TermId { get; private set; }
    public Guid? TeacherId { get; private set; } // Staff/Teacher assigned
    public string? TeacherName { get; private set; }
    public int Capacity { get; private set; }
    public int EnrolledCount { get; private set; } = 0;
    public string? RoomNumber { get; private set; }
    public string? Building { get; private set; }
    public string AcademicYear { get; private set; } = string.Empty;
    public string Shift { get; private set; } = string.Empty; // Morning, Afternoon, Evening
    public bool IsActive { get; private set; } = true;
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private ClassSection() { }

    public ClassSection(
        string sectionName,
        Guid courseId,
        string academicYear,
        string shift,
        int capacity,
        Guid? termId = null,
        Guid? teacherId = null,
        string? teacherName = null,
        string? roomNumber = null,
        string? building = null,
        string? remarks = null,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(sectionName))
            throw new ArgumentException("Section name is required.", nameof(sectionName));
        if (string.IsNullOrWhiteSpace(academicYear))
            throw new ArgumentException("Academic year is required.", nameof(academicYear));
        if (string.IsNullOrWhiteSpace(shift))
            throw new ArgumentException("Shift is required.", nameof(shift));
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));

        SectionName = sectionName.Trim();
        CourseId = courseId;
        AcademicYear = academicYear.Trim();
        Shift = shift.Trim();
        Capacity = capacity;
        TermId = termId;
        TeacherId = teacherId;
        TeacherName = string.IsNullOrWhiteSpace(teacherName) ? null : teacherName.Trim();
        RoomNumber = string.IsNullOrWhiteSpace(roomNumber) ? null : roomNumber.Trim();
        Building = string.IsNullOrWhiteSpace(building) ? null : building.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void AssignTeacher(Guid teacherId, string? teacherName = null, string? updatedBy = null)
    {
        TeacherId = teacherId;
        TeacherName = string.IsNullOrWhiteSpace(teacherName) ? null : teacherName.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateEnrollmentCount(int count, string? updatedBy = null)
    {
        if (count < 0)
            throw new ArgumentException("Enrollment count cannot be negative.", nameof(count));
        if (count > Capacity)
            throw new ArgumentException("Enrollment count cannot exceed capacity.", nameof(count));

        EnrolledCount = count;
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

    public void Deactivate(string? updatedBy = null)
    {
        IsActive = false;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}


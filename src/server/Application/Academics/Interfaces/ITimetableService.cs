using ERP.Domain.Academics.Entities;

namespace ERP.Application.Academics.Interfaces;

public interface ITimetableService
{
    Task<Guid> CreateAcademicTermAsync(
        string termName,
        string academicYear,
        DateTime startDate,
        DateTime endDate,
        string? remarks = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateClassSectionAsync(
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
        string? createdBy = null,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateTimetableSlotAsync(
        Guid classSectionId,
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        Guid? teacherId = null,
        string? teacherName = null,
        string? roomNumber = null,
        string? building = null,
        string? remarks = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default);

    Task<ConflictCheckResult> CheckConflictsAsync(
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        Guid? teacherId = null,
        string? roomNumber = null,
        string? building = null,
        Guid? excludeSlotId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<RoomAvailabilityDto>> GetAvailableRoomsAsync(
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        int minCapacity,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateRoomAsync(
        string roomNumber,
        RoomType type,
        int capacity,
        string? building = null,
        string? floor = null,
        bool hasProjector = false,
        bool hasComputerLab = false,
        bool hasWhiteboard = true,
        string? equipment = null,
        string? remarks = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default);
}

public sealed record ConflictCheckResult(
    bool HasConflicts,
    IReadOnlyCollection<ConflictDetail> Conflicts);

public sealed record ConflictDetail(
    ConflictType Type,
    string Description,
    Guid? SlotId = null,
    Guid? TeacherId = null,
    string? RoomNumber = null);

public enum ConflictType
{
    TeacherConflict = 0,
    RoomConflict = 1,
    BothConflict = 2
}

public sealed record RoomAvailabilityDto(
    Guid RoomId,
    string RoomNumber,
    string? Building,
    string? Floor,
    RoomType Type,
    int Capacity,
    bool HasProjector,
    bool HasComputerLab);


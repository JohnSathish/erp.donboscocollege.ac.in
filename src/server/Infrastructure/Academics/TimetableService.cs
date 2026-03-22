using ERP.Application.Academics.Interfaces;
using ERP.Domain.Academics.Entities;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Academics;

public class TimetableService : ITimetableService
{
    private readonly IAcademicTermRepository _termRepository;
    private readonly IClassSectionRepository _sectionRepository;
    private readonly ITimetableSlotRepository _slotRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger<TimetableService> _logger;

    public TimetableService(
        IAcademicTermRepository termRepository,
        IClassSectionRepository sectionRepository,
        ITimetableSlotRepository slotRepository,
        IRoomRepository roomRepository,
        ILogger<TimetableService> logger)
    {
        _termRepository = termRepository;
        _sectionRepository = sectionRepository;
        _slotRepository = slotRepository;
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<Guid> CreateAcademicTermAsync(
        string termName,
        string academicYear,
        DateTime startDate,
        DateTime endDate,
        string? remarks = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default)
    {
        var term = new AcademicTerm(termName, academicYear, startDate, endDate, remarks, createdBy);
        await _termRepository.AddAsync(term, cancellationToken);
        await _termRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created academic term {TermName} (ID: {TermId}) for {AcademicYear}.",
            termName, term.Id, academicYear);

        return term.Id;
    }

    public async Task<Guid> CreateClassSectionAsync(
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
        CancellationToken cancellationToken = default)
    {
        var section = new ClassSection(
            sectionName,
            courseId,
            academicYear,
            shift,
            capacity,
            termId,
            teacherId,
            teacherName,
            roomNumber,
            building,
            remarks,
            createdBy);

        await _sectionRepository.AddAsync(section, cancellationToken);
        await _sectionRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created class section {SectionName} (ID: {SectionId}) for course {CourseId}.",
            sectionName, section.Id, courseId);

        return section.Id;
    }

    public async Task<Guid> CreateTimetableSlotAsync(
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
        CancellationToken cancellationToken = default)
    {
        // Check for conflicts before creating
        var conflictCheck = await CheckConflictsAsync(
            dayOfWeek,
            startTime,
            endTime,
            teacherId,
            roomNumber,
            building,
            null,
            cancellationToken);

        if (conflictCheck.HasConflicts)
        {
            var conflictMessages = string.Join(", ", conflictCheck.Conflicts.Select(c => c.Description));
            throw new InvalidOperationException($"Cannot create timetable slot due to conflicts: {conflictMessages}");
        }

        var slot = new TimetableSlot(
            classSectionId,
            dayOfWeek,
            startTime,
            endTime,
            teacherId,
            teacherName,
            roomNumber,
            building,
            remarks,
            createdBy);

        await _slotRepository.AddAsync(slot, cancellationToken);
        await _slotRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created timetable slot (ID: {SlotId}) for class section {ClassSectionId} on {DayOfWeek}.",
            slot.Id, classSectionId, dayOfWeek);

        return slot.Id;
    }

    public async Task<ConflictCheckResult> CheckConflictsAsync(
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        Guid? teacherId = null,
        string? roomNumber = null,
        string? building = null,
        Guid? excludeSlotId = null,
        CancellationToken cancellationToken = default)
    {
        var conflictingSlots = await _slotRepository.GetConflictingSlotsAsync(
            dayOfWeek,
            startTime,
            endTime,
            teacherId,
            roomNumber,
            building,
            excludeSlotId,
            cancellationToken);

        if (!conflictingSlots.Any())
        {
            return new ConflictCheckResult(false, Array.Empty<ConflictDetail>());
        }

        var conflicts = new List<ConflictDetail>();

        foreach (var slot in conflictingSlots)
        {
            var conflictTypes = new List<ConflictType>();

            if (teacherId.HasValue && slot.TeacherId == teacherId)
            {
                conflictTypes.Add(ConflictType.TeacherConflict);
            }

            if (!string.IsNullOrWhiteSpace(roomNumber) && slot.RoomNumber == roomNumber)
            {
                if (string.IsNullOrWhiteSpace(building) || slot.Building == building)
                {
                    conflictTypes.Add(ConflictType.RoomConflict);
                }
            }

            if (conflictTypes.Count == 2)
            {
                conflicts.Add(new ConflictDetail(
                    ConflictType.BothConflict,
                    $"Teacher and room conflict with slot {slot.Id}",
                    slot.Id,
                    slot.TeacherId,
                    slot.RoomNumber));
            }
            else if (conflictTypes.Contains(ConflictType.TeacherConflict))
            {
                conflicts.Add(new ConflictDetail(
                    ConflictType.TeacherConflict,
                    $"Teacher conflict with slot {slot.Id}",
                    slot.Id,
                    slot.TeacherId,
                    null));
            }
            else if (conflictTypes.Contains(ConflictType.RoomConflict))
            {
                conflicts.Add(new ConflictDetail(
                    ConflictType.RoomConflict,
                    $"Room conflict with slot {slot.Id}",
                    slot.Id,
                    null,
                    slot.RoomNumber));
            }
        }

        return new ConflictCheckResult(true, conflicts);
    }

    public async Task<IReadOnlyCollection<RoomAvailabilityDto>> GetAvailableRoomsAsync(
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        int minCapacity,
        CancellationToken cancellationToken = default)
    {
        var availableRooms = await _roomRepository.GetAvailableRoomsAsync(
            minCapacity,
            dayOfWeek,
            startTime,
            endTime,
            cancellationToken);

        return availableRooms.Select(r => new RoomAvailabilityDto(
            r.Id,
            r.RoomNumber,
            r.Building,
            r.Floor,
            r.Type,
            r.Capacity,
            r.HasProjector,
            r.HasComputerLab)).ToList();
    }

    public async Task<Guid> CreateRoomAsync(
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
        CancellationToken cancellationToken = default)
    {
        // Check if room already exists
        var existing = await _roomRepository.GetByRoomNumberAsync(roomNumber, building, cancellationToken);
        if (existing != null)
        {
            throw new InvalidOperationException($"Room {roomNumber} already exists in building {building ?? "N/A"}.");
        }

        var room = new Room(
            roomNumber,
            type,
            capacity,
            building,
            floor,
            hasProjector,
            hasComputerLab,
            hasWhiteboard,
            equipment,
            remarks,
            createdBy);

        await _roomRepository.AddAsync(room, cancellationToken);
        await _roomRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created room {RoomNumber} (ID: {RoomId}) in building {Building}.",
            roomNumber, room.Id, building ?? "N/A");

        return room.Id;
    }
}


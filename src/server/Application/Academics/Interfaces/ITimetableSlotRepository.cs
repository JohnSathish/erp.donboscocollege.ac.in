using ERP.Domain.Academics.Entities;

namespace ERP.Application.Academics.Interfaces;

public interface ITimetableSlotRepository
{
    Task<TimetableSlot?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TimetableSlot?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TimetableSlot>> GetByClassSectionIdAsync(Guid classSectionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TimetableSlot>> GetByTeacherIdAsync(Guid teacherId, DayOfWeek? dayOfWeek = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TimetableSlot>> GetByRoomAsync(string roomNumber, string? building = null, DayOfWeek? dayOfWeek = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TimetableSlot>> GetConflictingSlotsAsync(DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime, Guid? teacherId = null, string? roomNumber = null, string? building = null, Guid? excludeSlotId = null, CancellationToken cancellationToken = default);
    Task<TimetableSlot> AddAsync(TimetableSlot slot, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TimetableSlot> slots, CancellationToken cancellationToken = default);
    Task UpdateAsync(TimetableSlot slot, CancellationToken cancellationToken = default);
    Task DeleteAsync(TimetableSlot slot, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<TimetableSlot> slots, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


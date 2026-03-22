using ERP.Application.Academics.Interfaces;
using ERP.Domain.Academics.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Academics;

public class TimetableSlotRepository(ApplicationDbContext context) : ITimetableSlotRepository
{
    public async Task<TimetableSlot?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.TimetableSlots
            .Include(x => x.ClassSection)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<TimetableSlot?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.TimetableSlots
            .Include(x => x.ClassSection)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<TimetableSlot>> GetByClassSectionIdAsync(Guid classSectionId, CancellationToken cancellationToken = default)
    {
        return await context.TimetableSlots
            .Include(x => x.ClassSection)
            .Where(x => x.ClassSectionId == classSectionId && x.IsActive)
            .OrderBy(x => x.DayOfWeek)
            .ThenBy(x => x.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TimetableSlot>> GetByTeacherIdAsync(Guid teacherId, DayOfWeek? dayOfWeek = null, CancellationToken cancellationToken = default)
    {
        var query = context.TimetableSlots
            .Include(x => x.ClassSection)
            .Where(x => x.TeacherId == teacherId && x.IsActive);

        if (dayOfWeek.HasValue)
            query = query.Where(x => x.DayOfWeek == dayOfWeek.Value);

        return await query
            .OrderBy(x => x.DayOfWeek)
            .ThenBy(x => x.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TimetableSlot>> GetByRoomAsync(string roomNumber, string? building = null, DayOfWeek? dayOfWeek = null, CancellationToken cancellationToken = default)
    {
        var query = context.TimetableSlots
            .Include(x => x.ClassSection)
            .Where(x => x.RoomNumber == roomNumber && x.IsActive);

        if (!string.IsNullOrWhiteSpace(building))
            query = query.Where(x => x.Building == building);
        if (dayOfWeek.HasValue)
            query = query.Where(x => x.DayOfWeek == dayOfWeek.Value);

        return await query
            .OrderBy(x => x.DayOfWeek)
            .ThenBy(x => x.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TimetableSlot>> GetConflictingSlotsAsync(
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        Guid? teacherId = null,
        string? roomNumber = null,
        string? building = null,
        Guid? excludeSlotId = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.TimetableSlots
            .Where(x => x.DayOfWeek == dayOfWeek && x.IsActive);

        if (excludeSlotId.HasValue)
            query = query.Where(x => x.Id != excludeSlotId.Value);

        // Time overlap detection: (StartTime < endTime) && (EndTime > startTime)
        query = query.Where(x => x.StartTime < endTime && x.EndTime > startTime);

        if (teacherId.HasValue)
            query = query.Where(x => x.TeacherId == teacherId);

        if (!string.IsNullOrWhiteSpace(roomNumber))
        {
            query = query.Where(x => x.RoomNumber == roomNumber);
            if (!string.IsNullOrWhiteSpace(building))
                query = query.Where(x => x.Building == building);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TimetableSlot> AddAsync(TimetableSlot slot, CancellationToken cancellationToken = default)
    {
        await context.TimetableSlots.AddAsync(slot, cancellationToken);
        return slot;
    }

    public async Task AddRangeAsync(IEnumerable<TimetableSlot> slots, CancellationToken cancellationToken = default)
    {
        await context.TimetableSlots.AddRangeAsync(slots, cancellationToken);
    }

    public Task UpdateAsync(TimetableSlot slot, CancellationToken cancellationToken = default)
    {
        context.TimetableSlots.Update(slot);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TimetableSlot slot, CancellationToken cancellationToken = default)
    {
        context.TimetableSlots.Remove(slot);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TimetableSlot> slots, CancellationToken cancellationToken = default)
    {
        context.TimetableSlots.RemoveRange(slots);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}


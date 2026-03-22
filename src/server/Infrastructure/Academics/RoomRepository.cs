using ERP.Application.Academics.Interfaces;
using ERP.Domain.Academics.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Academics;

public class RoomRepository(ApplicationDbContext context) : IRoomRepository
{
    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Rooms
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Room?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Rooms
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Room?> GetByRoomNumberAsync(string roomNumber, string? building = null, CancellationToken cancellationToken = default)
    {
        var query = context.Rooms
            .Where(x => x.RoomNumber == roomNumber);

        if (!string.IsNullOrWhiteSpace(building))
            query = query.Where(x => x.Building == building);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Room>> GetByTypeAsync(RoomType type, CancellationToken cancellationToken = default)
    {
        return await context.Rooms
            .Where(x => x.Type == type && x.IsActive)
            .OrderBy(x => x.Building)
            .ThenBy(x => x.RoomNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Room>> GetAvailableRoomsAsync(
        int minCapacity,
        DayOfWeek? dayOfWeek = null,
        TimeOnly? startTime = null,
        TimeOnly? endTime = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Rooms
            .Where(x => x.IsActive && x.Capacity >= minCapacity);

        if (dayOfWeek.HasValue && startTime.HasValue && endTime.HasValue)
        {
            var conflictingRoomNumbers = await context.TimetableSlots
                .Where(x => x.DayOfWeek == dayOfWeek.Value &&
                           x.IsActive &&
                           x.StartTime < endTime.Value &&
                           x.EndTime > startTime.Value &&
                           x.RoomNumber != null)
                .Select(x => new { x.RoomNumber, x.Building })
                .Distinct()
                .ToListAsync(cancellationToken);

            if (conflictingRoomNumbers.Any())
            {
                foreach (var conflict in conflictingRoomNumbers)
                {
                    if (!string.IsNullOrWhiteSpace(conflict.Building))
                    {
                        query = query.Where(x => !(x.RoomNumber == conflict.RoomNumber && x.Building == conflict.Building));
                    }
                    else
                    {
                        query = query.Where(x => x.RoomNumber != conflict.RoomNumber);
                    }
                }
            }
        }

        return await query
            .OrderBy(x => x.Building)
            .ThenBy(x => x.RoomNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Room>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Rooms
            .Where(x => x.IsActive)
            .OrderBy(x => x.Building)
            .ThenBy(x => x.RoomNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<Room> AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        await context.Rooms.AddAsync(room, cancellationToken);
        return room;
    }

    public Task UpdateAsync(Room room, CancellationToken cancellationToken = default)
    {
        context.Rooms.Update(room);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}


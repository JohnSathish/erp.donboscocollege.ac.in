using ERP.Domain.Academics.Entities;

namespace ERP.Application.Academics.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Room?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Room?> GetByRoomNumberAsync(string roomNumber, string? building = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Room>> GetByTypeAsync(RoomType type, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Room>> GetAvailableRoomsAsync(int minCapacity, DayOfWeek? dayOfWeek = null, TimeOnly? startTime = null, TimeOnly? endTime = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Room>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Room> AddAsync(Room room, CancellationToken cancellationToken = default);
    Task UpdateAsync(Room room, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


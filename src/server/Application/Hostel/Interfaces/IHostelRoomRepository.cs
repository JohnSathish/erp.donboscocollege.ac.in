using ERP.Domain.Hostel.Entities;

namespace ERP.Application.Hostel.Interfaces;

public interface IHostelRoomRepository
{
    Task<HostelRoom?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<HostelRoom?> GetByRoomNumberAsync(string blockName, string roomNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<HostelRoom>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<HostelRoom> Rooms, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? blockName = null,
        string? roomType = null,
        RoomStatus? status = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    Task<HostelRoom> AddAsync(HostelRoom room, CancellationToken cancellationToken = default);
    Task UpdateAsync(HostelRoom room, CancellationToken cancellationToken = default);
    Task<bool> RoomNumberExistsAsync(string blockName, string roomNumber, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}





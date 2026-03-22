using ERP.Domain.Hostel.Entities;

namespace ERP.Application.Hostel.Interfaces;

public interface IRoomAllocationRepository
{
    Task<RoomAllocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RoomAllocation?> GetActiveAllocationByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RoomAllocation>> GetAllocationsByRoomIdAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RoomAllocation>> GetAllocationsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<RoomAllocation> Allocations, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? roomId = null,
        Guid? studentId = null,
        AllocationStatus? status = null,
        CancellationToken cancellationToken = default);
    Task<RoomAllocation> AddAsync(RoomAllocation allocation, CancellationToken cancellationToken = default);
    Task UpdateAsync(RoomAllocation allocation, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}





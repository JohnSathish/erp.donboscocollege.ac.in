using ERP.Application.Hostel.Interfaces;
using ERP.Domain.Hostel.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Hostel;

public class RoomAllocationRepository : IRoomAllocationRepository
{
    private readonly ApplicationDbContext _context;

    public RoomAllocationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<RoomAllocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.RoomAllocations
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public Task<RoomAllocation?> GetActiveAllocationByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return _context.RoomAllocations
            .FirstOrDefaultAsync(a => a.StudentId == studentId && a.Status == AllocationStatus.Active, cancellationToken);
    }

    public Task<IReadOnlyCollection<RoomAllocation>> GetAllocationsByRoomIdAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<RoomAllocation>>(
            _context.RoomAllocations
                .AsNoTracking()
                .Where(a => a.RoomId == roomId)
                .OrderByDescending(a => a.AllocationDate)
                .ToList());
    }

    public Task<IReadOnlyCollection<RoomAllocation>> GetAllocationsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<RoomAllocation>>(
            _context.RoomAllocations
                .AsNoTracking()
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.AllocationDate)
                .ToList());
    }

    public async Task<(IReadOnlyCollection<RoomAllocation> Allocations, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? roomId = null,
        Guid? studentId = null,
        AllocationStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.RoomAllocations.AsNoTracking();

        if (roomId.HasValue)
        {
            query = query.Where(a => a.RoomId == roomId.Value);
        }

        if (studentId.HasValue)
        {
            query = query.Where(a => a.StudentId == studentId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(a => a.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var allocations = await query
            .OrderByDescending(a => a.AllocationDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (allocations, totalCount);
    }

    public async Task<RoomAllocation> AddAsync(RoomAllocation allocation, CancellationToken cancellationToken = default)
    {
        await _context.RoomAllocations.AddAsync(allocation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return allocation;
    }

    public async Task UpdateAsync(RoomAllocation allocation, CancellationToken cancellationToken = default)
    {
        _context.RoomAllocations.Update(allocation);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}





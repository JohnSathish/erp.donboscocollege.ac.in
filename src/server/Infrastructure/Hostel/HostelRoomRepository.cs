using ERP.Application.Hostel.Interfaces;
using ERP.Domain.Hostel.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Hostel;

public class HostelRoomRepository : IHostelRoomRepository
{
    private readonly ApplicationDbContext _context;

    public HostelRoomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<HostelRoom?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.HostelRooms
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task<HostelRoom?> GetByRoomNumberAsync(string blockName, string roomNumber, CancellationToken cancellationToken = default)
    {
        return _context.HostelRooms
            .FirstOrDefaultAsync(r => r.BlockName == blockName && r.RoomNumber == roomNumber, cancellationToken);
    }

    public Task<IReadOnlyCollection<HostelRoom>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<HostelRoom>>(
            _context.HostelRooms
                .AsNoTracking()
                .ToList());
    }

    public async Task<(IReadOnlyCollection<HostelRoom> Rooms, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? blockName = null,
        string? roomType = null,
        RoomStatus? status = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.HostelRooms.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(blockName))
        {
            query = query.Where(r => r.BlockName == blockName);
        }

        if (!string.IsNullOrWhiteSpace(roomType))
        {
            query = query.Where(r => r.RoomType == roomType);
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.Trim();
            query = query.Where(r =>
                r.RoomNumber.Contains(search) ||
                r.BlockName.Contains(search) ||
                r.FloorNumber.Contains(search) ||
                (r.Facilities != null && r.Facilities.Contains(search)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var rooms = await query
            .OrderBy(r => r.BlockName)
            .ThenBy(r => r.RoomNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (rooms, totalCount);
    }

    public async Task<HostelRoom> AddAsync(HostelRoom room, CancellationToken cancellationToken = default)
    {
        await _context.HostelRooms.AddAsync(room, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return room;
    }

    public async Task UpdateAsync(HostelRoom room, CancellationToken cancellationToken = default)
    {
        _context.HostelRooms.Update(room);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> RoomNumberExistsAsync(string blockName, string roomNumber, CancellationToken cancellationToken = default)
    {
        return _context.HostelRooms
            .AnyAsync(r => r.BlockName == blockName && r.RoomNumber == roomNumber, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}





using ERP.Application.Hostel.Interfaces;
using MediatR;

namespace ERP.Application.Hostel.Queries.ListHostelRooms;

public sealed class ListHostelRoomsQueryHandler : IRequestHandler<ListHostelRoomsQuery, ListHostelRoomsResult>
{
    private readonly IHostelRoomRepository _roomRepository;

    public ListHostelRoomsQueryHandler(IHostelRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<ListHostelRoomsResult> Handle(ListHostelRoomsQuery request, CancellationToken cancellationToken)
    {
        var (rooms, totalCount) = await _roomRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.BlockName,
            request.RoomType,
            request.Status,
            request.SearchTerm,
            cancellationToken);

        var roomDtos = rooms.Select(r => new HostelRoomDto(
            r.Id,
            r.RoomNumber,
            r.BlockName,
            r.FloorNumber,
            r.Capacity,
            r.OccupiedBeds,
            r.AvailableBeds,
            r.RoomType,
            r.MonthlyRent,
            r.Facilities,
            r.Status.ToString(),
            r.Notes,
            r.CreatedOnUtc,
            r.CreatedBy)).ToList();

        return new ListHostelRoomsResult(
            roomDtos,
            totalCount,
            request.Page,
            request.PageSize);
    }
}





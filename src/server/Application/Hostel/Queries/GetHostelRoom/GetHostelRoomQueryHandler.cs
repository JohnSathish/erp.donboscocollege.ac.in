using ERP.Application.Hostel.Interfaces;
using MediatR;

namespace ERP.Application.Hostel.Queries.GetHostelRoom;

public sealed class GetHostelRoomQueryHandler : IRequestHandler<GetHostelRoomQuery, GetHostelRoomResult?>
{
    private readonly IHostelRoomRepository _repository;

    public GetHostelRoomQueryHandler(IHostelRoomRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetHostelRoomResult?> Handle(GetHostelRoomQuery request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIdAsync(request.RoomId, cancellationToken);
        
        if (room == null)
        {
            return null;
        }

        return new GetHostelRoomResult(
            room.Id,
            room.RoomNumber,
            room.BlockName,
            room.FloorNumber,
            room.Capacity,
            room.OccupiedBeds,
            room.AvailableBeds,
            room.RoomType,
            room.MonthlyRent,
            room.Facilities,
            room.Status.ToString(),
            room.Notes,
            room.CreatedOnUtc,
            room.CreatedBy,
            room.UpdatedOnUtc,
            room.UpdatedBy);
    }
}


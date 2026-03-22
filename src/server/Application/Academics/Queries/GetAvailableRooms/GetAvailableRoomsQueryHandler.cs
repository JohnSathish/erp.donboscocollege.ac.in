using ERP.Application.Academics.Interfaces;
using MediatR;

namespace ERP.Application.Academics.Queries.GetAvailableRooms;

public sealed class GetAvailableRoomsQueryHandler : IRequestHandler<GetAvailableRoomsQuery, IReadOnlyCollection<RoomAvailabilityDto>>
{
    private readonly ITimetableService _timetableService;

    public GetAvailableRoomsQueryHandler(ITimetableService timetableService)
    {
        _timetableService = timetableService;
    }

    public async Task<IReadOnlyCollection<RoomAvailabilityDto>> Handle(GetAvailableRoomsQuery request, CancellationToken cancellationToken)
    {
        return await _timetableService.GetAvailableRoomsAsync(
            request.DayOfWeek,
            request.StartTime,
            request.EndTime,
            request.MinCapacity,
            cancellationToken);
    }
}


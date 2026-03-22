using MediatR;
using ERP.Application.Academics.Interfaces;

namespace ERP.Application.Academics.Queries.GetAvailableRooms;

public sealed record GetAvailableRoomsQuery(
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int MinCapacity) : IRequest<IReadOnlyCollection<RoomAvailabilityDto>>;


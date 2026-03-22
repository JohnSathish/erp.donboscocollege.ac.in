using MediatR;

namespace ERP.Application.Hostel.Commands.UpdateHostelRoom;

public sealed record UpdateHostelRoomCommand(
    Guid RoomId,
    string? BlockName = null,
    string? FloorNumber = null,
    int? Capacity = null,
    string? RoomType = null,
    decimal? MonthlyRent = null,
    string? Facilities = null,
    string? Notes = null,
    string? UpdatedBy = null) : IRequest;


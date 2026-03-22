using MediatR;

namespace ERP.Application.Hostel.Commands.CreateHostelRoom;

public sealed record CreateHostelRoomCommand(
    string RoomNumber,
    string BlockName,
    string FloorNumber,
    int Capacity,
    string RoomType,
    decimal? MonthlyRent = null,
    string? Facilities = null,
    string? Notes = null,
    string? CreatedBy = null) : IRequest<Guid>;





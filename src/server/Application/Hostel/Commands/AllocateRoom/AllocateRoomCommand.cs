using MediatR;

namespace ERP.Application.Hostel.Commands.AllocateRoom;

public sealed record AllocateRoomCommand(
    Guid RoomId,
    Guid StudentId,
    DateTime AllocationDate,
    decimal? MonthlyRent = null,
    string? BedNumber = null,
    string? Remarks = null,
    string? AllocatedBy = null) : IRequest<Guid>;





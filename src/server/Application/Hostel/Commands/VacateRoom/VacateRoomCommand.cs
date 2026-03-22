using MediatR;

namespace ERP.Application.Hostel.Commands.VacateRoom;

public sealed record VacateRoomCommand(
    Guid AllocationId,
    DateTime VacatedDate,
    string? VacatedBy = null,
    string? Remarks = null) : IRequest<Unit>;





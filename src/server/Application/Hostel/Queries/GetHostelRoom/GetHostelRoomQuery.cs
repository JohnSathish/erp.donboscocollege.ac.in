using MediatR;

namespace ERP.Application.Hostel.Queries.GetHostelRoom;

public sealed record GetHostelRoomQuery(Guid RoomId) : IRequest<GetHostelRoomResult?>;

public sealed record GetHostelRoomResult(
    Guid Id,
    string RoomNumber,
    string BlockName,
    string FloorNumber,
    int Capacity,
    int OccupiedBeds,
    int AvailableBeds,
    string RoomType,
    decimal? MonthlyRent,
    string? Facilities,
    string Status,
    string? Notes,
    DateTime CreatedOnUtc,
    string? CreatedBy,
    DateTime? UpdatedOnUtc,
    string? UpdatedBy);


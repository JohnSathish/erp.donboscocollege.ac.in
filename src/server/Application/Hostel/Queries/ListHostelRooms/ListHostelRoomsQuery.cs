using MediatR;
using ERP.Domain.Hostel.Entities;

namespace ERP.Application.Hostel.Queries.ListHostelRooms;

public sealed record ListHostelRoomsQuery(
    int Page = 1,
    int PageSize = 50,
    string? BlockName = null,
    string? RoomType = null,
    RoomStatus? Status = null,
    string? SearchTerm = null) : IRequest<ListHostelRoomsResult>;

public sealed record ListHostelRoomsResult(
    IReadOnlyCollection<HostelRoomDto> Rooms,
    int TotalCount,
    int Page,
    int PageSize);

public sealed record HostelRoomDto(
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
    string? CreatedBy);





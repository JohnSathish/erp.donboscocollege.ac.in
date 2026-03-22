using MediatR;
using ERP.Domain.Academics.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Academics.Commands.CreateRoom;

public sealed record CreateRoomCommand(
    [Required] string RoomNumber,
    [Required] RoomType Type,
    [Required] int Capacity,
    string? Building = null,
    string? Floor = null,
    bool HasProjector = false,
    bool HasComputerLab = false,
    bool HasWhiteboard = true,
    string? Equipment = null,
    string? Remarks = null,
    string? CreatedBy = null) : IRequest<Guid>;


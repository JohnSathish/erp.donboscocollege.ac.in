using ERP.Application.Hostel.Interfaces;
using ERP.Domain.Hostel.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Hostel.Commands.CreateHostelRoom;

public sealed class CreateHostelRoomCommandHandler : IRequestHandler<CreateHostelRoomCommand, Guid>
{
    private readonly IHostelRoomRepository _roomRepository;
    private readonly ILogger<CreateHostelRoomCommandHandler> _logger;

    public CreateHostelRoomCommandHandler(
        IHostelRoomRepository roomRepository,
        ILogger<CreateHostelRoomCommandHandler> logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateHostelRoomCommand request, CancellationToken cancellationToken)
    {
        // Check if room number already exists in the block
        var exists = await _roomRepository.RoomNumberExistsAsync(request.BlockName, request.RoomNumber, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException(
                $"Room '{request.RoomNumber}' already exists in block '{request.BlockName}'.");
        }

        var room = new HostelRoom(
            request.RoomNumber,
            request.BlockName,
            request.FloorNumber,
            request.Capacity,
            request.RoomType,
            request.MonthlyRent,
            request.Facilities,
            request.Notes,
            request.CreatedBy);

        await _roomRepository.AddAsync(room, cancellationToken);

        _logger.LogInformation(
            "Created hostel room {RoomNumber} in block {BlockName}",
            room.RoomNumber,
            room.BlockName);

        return room.Id;
    }
}





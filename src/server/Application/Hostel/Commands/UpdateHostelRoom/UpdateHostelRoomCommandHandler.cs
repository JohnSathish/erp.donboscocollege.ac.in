using ERP.Application.Hostel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Hostel.Commands.UpdateHostelRoom;

public sealed class UpdateHostelRoomCommandHandler : IRequestHandler<UpdateHostelRoomCommand, Unit>
{
    private readonly IHostelRoomRepository _repository;
    private readonly ILogger<UpdateHostelRoomCommandHandler> _logger;

    public UpdateHostelRoomCommandHandler(
        IHostelRoomRepository repository,
        ILogger<UpdateHostelRoomCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateHostelRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIdAsync(request.RoomId, cancellationToken);
        
        if (room == null)
        {
            throw new InvalidOperationException($"Hostel room with ID '{request.RoomId}' not found.");
        }

        room.UpdateDetails(
            request.BlockName ?? room.BlockName,
            request.FloorNumber ?? room.FloorNumber,
            request.Capacity ?? room.Capacity,
            request.RoomType ?? room.RoomType,
            request.MonthlyRent ?? room.MonthlyRent,
            request.Facilities ?? room.Facilities,
            request.Notes ?? room.Notes,
            request.UpdatedBy);

        await _repository.UpdateAsync(room, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Updated hostel room {RoomId} ({BlockName}-{RoomNumber}) by {UpdatedBy}",
            room.Id,
            room.BlockName,
            room.RoomNumber,
            request.UpdatedBy ?? "System");

        return Unit.Value;
    }
}


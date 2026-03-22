using ERP.Application.Academics.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Academics.Commands.CreateRoom;

public sealed class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
{
    private readonly ITimetableService _timetableService;
    private readonly ILogger<CreateRoomCommandHandler> _logger;

    public CreateRoomCommandHandler(
        ITimetableService timetableService,
        ILogger<CreateRoomCommandHandler> logger)
    {
        _timetableService = timetableService;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        return await _timetableService.CreateRoomAsync(
            request.RoomNumber,
            request.Type,
            request.Capacity,
            request.Building,
            request.Floor,
            request.HasProjector,
            request.HasComputerLab,
            request.HasWhiteboard,
            request.Equipment,
            request.Remarks,
            request.CreatedBy,
            cancellationToken);
    }
}


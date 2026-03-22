using ERP.Application.Hostel.Interfaces;
using ERP.Domain.Hostel.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Hostel.Commands.VacateRoom;

public sealed class VacateRoomCommandHandler : IRequestHandler<VacateRoomCommand, Unit>
{
    private readonly IRoomAllocationRepository _allocationRepository;
    private readonly IHostelRoomRepository _roomRepository;
    private readonly ILogger<VacateRoomCommandHandler> _logger;

    public VacateRoomCommandHandler(
        IRoomAllocationRepository allocationRepository,
        IHostelRoomRepository roomRepository,
        ILogger<VacateRoomCommandHandler> logger)
    {
        _allocationRepository = allocationRepository;
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(VacateRoomCommand request, CancellationToken cancellationToken)
    {
        var allocation = await _allocationRepository.GetByIdAsync(request.AllocationId, cancellationToken);
        if (allocation == null)
        {
            throw new InvalidOperationException($"Room allocation with ID '{request.AllocationId}' not found.");
        }

        if (allocation.Status != AllocationStatus.Active)
        {
            throw new InvalidOperationException("Only active allocations can be vacated.");
        }

        // Vacate the allocation
        allocation.Vacate(request.VacatedDate, request.VacatedBy, request.Remarks);
        await _allocationRepository.UpdateAsync(allocation, cancellationToken);

        // Update room occupancy
        var room = await _roomRepository.GetByIdAsync(allocation.RoomId, cancellationToken);
        if (room != null)
        {
            room.VacateBed();
            await _roomRepository.UpdateAsync(room, cancellationToken);
        }

        _logger.LogInformation(
            "Vacated room allocation {AllocationId}",
            request.AllocationId);

        return Unit.Value;
    }
}





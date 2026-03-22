using ERP.Application.Hostel.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Hostel.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Hostel.Commands.AllocateRoom;

public sealed class AllocateRoomCommandHandler : IRequestHandler<AllocateRoomCommand, Guid>
{
    private readonly IHostelRoomRepository _roomRepository;
    private readonly IRoomAllocationRepository _allocationRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<AllocateRoomCommandHandler> _logger;

    public AllocateRoomCommandHandler(
        IHostelRoomRepository roomRepository,
        IRoomAllocationRepository allocationRepository,
        IStudentRepository studentRepository,
        ILogger<AllocateRoomCommandHandler> logger)
    {
        _roomRepository = roomRepository;
        _allocationRepository = allocationRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(AllocateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room == null)
        {
            throw new InvalidOperationException($"Room with ID '{request.RoomId}' not found.");
        }

        if (room.AvailableBeds <= 0)
        {
            throw new InvalidOperationException($"No available beds in room '{room.RoomNumber}'.");
        }

        // Check if student already has an active allocation
        var existingAllocation = await _allocationRepository.GetActiveAllocationByStudentIdAsync(request.StudentId, cancellationToken);
        if (existingAllocation != null)
        {
            throw new InvalidOperationException(
                $"Student already has an active room allocation in room '{existingAllocation.RoomId}'.");
        }

        // Verify student exists
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            throw new InvalidOperationException($"Student with ID '{request.StudentId}' not found.");
        }

        var allocation = new RoomAllocation(
            request.RoomId,
            request.StudentId,
            request.AllocationDate,
            request.MonthlyRent ?? room.MonthlyRent,
            request.BedNumber,
            request.Remarks,
            request.AllocatedBy);

        await _allocationRepository.AddAsync(allocation, cancellationToken);

        // Update room occupancy
        room.AllocateBed();
        await _roomRepository.UpdateAsync(room, cancellationToken);

        _logger.LogInformation(
            "Allocated room {RoomId} to student {StudentId}",
            request.RoomId,
            request.StudentId);

        return allocation.Id;
    }
}





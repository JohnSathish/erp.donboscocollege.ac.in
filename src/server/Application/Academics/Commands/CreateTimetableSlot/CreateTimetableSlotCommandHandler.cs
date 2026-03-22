using ERP.Application.Academics.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Academics.Commands.CreateTimetableSlot;

public sealed class CreateTimetableSlotCommandHandler : IRequestHandler<CreateTimetableSlotCommand, CreateTimetableSlotResult>
{
    private readonly ITimetableService _timetableService;
    private readonly ILogger<CreateTimetableSlotCommandHandler> _logger;

    public CreateTimetableSlotCommandHandler(
        ITimetableService timetableService,
        ILogger<CreateTimetableSlotCommandHandler> logger)
    {
        _timetableService = timetableService;
        _logger = logger;
    }

    public async Task<CreateTimetableSlotResult> Handle(CreateTimetableSlotCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var slotId = await _timetableService.CreateTimetableSlotAsync(
                request.ClassSectionId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                request.TeacherId,
                request.TeacherName,
                request.RoomNumber,
                request.Building,
                request.Remarks,
                request.CreatedBy,
                cancellationToken);

            return new CreateTimetableSlotResult(slotId, true);
        }
        catch (InvalidOperationException ex)
        {
            // Check conflicts for detailed error
            var conflicts = await _timetableService.CheckConflictsAsync(
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                request.TeacherId,
                request.RoomNumber,
                request.Building,
                null,
                cancellationToken);

            return new CreateTimetableSlotResult(
                Guid.Empty,
                false,
                ex.Message,
                conflicts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating timetable slot for class section {ClassSectionId}.", request.ClassSectionId);
            return new CreateTimetableSlotResult(Guid.Empty, false, ex.Message);
        }
    }
}


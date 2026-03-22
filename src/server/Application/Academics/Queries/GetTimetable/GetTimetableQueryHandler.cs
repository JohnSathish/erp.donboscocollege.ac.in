using ERP.Application.Academics.Interfaces;
using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Academics.Queries.GetTimetable;

public sealed class GetTimetableQueryHandler : IRequestHandler<GetTimetableQuery, IReadOnlyCollection<TimetableSlotDto>>
{
    private readonly ITimetableSlotRepository _slotRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<GetTimetableQueryHandler> _logger;

    public GetTimetableQueryHandler(
        ITimetableSlotRepository slotRepository,
        ICourseRepository courseRepository,
        ILogger<GetTimetableQueryHandler> logger)
    {
        _slotRepository = slotRepository;
        _courseRepository = courseRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<TimetableSlotDto>> Handle(GetTimetableQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Domain.Academics.Entities.TimetableSlot> slots;

        if (request.ClassSectionId.HasValue)
        {
            slots = await _slotRepository.GetByClassSectionIdAsync(request.ClassSectionId.Value, cancellationToken);
        }
        else if (request.TeacherId.HasValue)
        {
            slots = await _slotRepository.GetByTeacherIdAsync(request.TeacherId.Value, request.DayOfWeek, cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(request.RoomNumber))
        {
            slots = await _slotRepository.GetByRoomAsync(request.RoomNumber, request.Building, request.DayOfWeek, cancellationToken);
        }
        else
        {
            _logger.LogWarning("GetTimetableQuery requires at least one filter parameter.");
            return Array.Empty<TimetableSlotDto>();
        }

        var result = new List<TimetableSlotDto>();

        foreach (var slot in slots)
        {
            string? courseName = null;
            if (slot.ClassSection != null)
            {
                var course = await _courseRepository.GetByIdAsync(slot.ClassSection.CourseId, cancellationToken);
                courseName = course?.Name;
            }

            result.Add(new TimetableSlotDto(
                slot.Id,
                slot.ClassSectionId,
                slot.ClassSection?.SectionName ?? "Unknown",
                courseName,
                slot.DayOfWeek,
                slot.StartTime,
                slot.EndTime,
                slot.RoomNumber,
                slot.Building,
                slot.TeacherId,
                slot.TeacherName,
                slot.Remarks));
        }

        return result;
    }
}


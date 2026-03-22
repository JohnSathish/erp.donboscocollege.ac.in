using ERP.Application.Academics.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Academics.Commands.CreateClassSection;

public sealed class CreateClassSectionCommandHandler : IRequestHandler<CreateClassSectionCommand, Guid>
{
    private readonly ITimetableService _timetableService;
    private readonly ILogger<CreateClassSectionCommandHandler> _logger;

    public CreateClassSectionCommandHandler(
        ITimetableService timetableService,
        ILogger<CreateClassSectionCommandHandler> logger)
    {
        _timetableService = timetableService;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateClassSectionCommand request, CancellationToken cancellationToken)
    {
        return await _timetableService.CreateClassSectionAsync(
            request.SectionName,
            request.CourseId,
            request.AcademicYear,
            request.Shift,
            request.Capacity,
            request.TermId,
            request.TeacherId,
            request.TeacherName,
            request.RoomNumber,
            request.Building,
            request.Remarks,
            request.CreatedBy,
            cancellationToken);
    }
}


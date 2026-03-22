using ERP.Application.Academics.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Academics.Commands.CreateAcademicTerm;

public sealed class CreateAcademicTermCommandHandler : IRequestHandler<CreateAcademicTermCommand, Guid>
{
    private readonly ITimetableService _timetableService;
    private readonly ILogger<CreateAcademicTermCommandHandler> _logger;

    public CreateAcademicTermCommandHandler(
        ITimetableService timetableService,
        ILogger<CreateAcademicTermCommandHandler> logger)
    {
        _timetableService = timetableService;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateAcademicTermCommand request, CancellationToken cancellationToken)
    {
        return await _timetableService.CreateAcademicTermAsync(
            request.TermName,
            request.AcademicYear,
            request.StartDate,
            request.EndDate,
            request.Remarks,
            request.CreatedBy,
            cancellationToken);
    }
}


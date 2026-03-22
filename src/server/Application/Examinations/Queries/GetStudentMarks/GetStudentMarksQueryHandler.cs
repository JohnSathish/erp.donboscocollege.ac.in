using ERP.Application.Examinations.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Examinations.Queries.GetStudentMarks;

public class GetStudentMarksQueryHandler : IRequestHandler<GetStudentMarksQuery, IReadOnlyCollection<MarkEntryDto>>
{
    private readonly IMarkEntryRepository _markEntryRepository;
    private readonly ILogger<GetStudentMarksQueryHandler> _logger;

    public GetStudentMarksQueryHandler(
        IMarkEntryRepository markEntryRepository,
        ILogger<GetStudentMarksQueryHandler> logger)
    {
        _markEntryRepository = markEntryRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<MarkEntryDto>> Handle(
        GetStudentMarksQuery request,
        CancellationToken cancellationToken)
    {
        var markEntries = await _markEntryRepository.GetByStudentIdAsync(
            request.StudentId,
            request.AcademicTermId,
            cancellationToken);

        return markEntries
            .Select(me => new MarkEntryDto(
                me.Id,
                me.AssessmentComponentId,
                me.AssessmentComponent.Name,
                me.AssessmentComponent.AssessmentId,
                me.AssessmentComponent.Assessment.Name,
                me.AssessmentComponent.Assessment.Code,
                me.MarksObtained,
                me.Percentage,
                me.Grade,
                me.Remarks,
                me.Status.ToString(),
                me.IsAbsent,
                me.IsExempted,
                me.EnteredOnUtc,
                me.EnteredBy,
                me.ApprovedOnUtc,
                me.ApprovedBy))
            .ToList();
    }
}


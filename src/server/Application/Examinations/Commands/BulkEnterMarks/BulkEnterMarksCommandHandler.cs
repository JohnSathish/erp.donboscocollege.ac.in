using ERP.Application.Examinations.Commands.EnterMarks;
using ERP.Application.Examinations.Interfaces;
using ERP.Domain.Examinations.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Examinations.Commands.BulkEnterMarks;

public class BulkEnterMarksCommandHandler : IRequestHandler<BulkEnterMarksCommand, int>
{
    private readonly IMarkEntryRepository _markEntryRepository;
    private readonly IAssessmentComponentRepository _componentRepository;
    private readonly IGradingService _gradingService;
    private readonly IMediator _mediator;
    private readonly ILogger<BulkEnterMarksCommandHandler> _logger;

    public BulkEnterMarksCommandHandler(
        IMarkEntryRepository markEntryRepository,
        IAssessmentComponentRepository componentRepository,
        IGradingService gradingService,
        IMediator mediator,
        ILogger<BulkEnterMarksCommandHandler> logger)
    {
        _markEntryRepository = markEntryRepository;
        _componentRepository = componentRepository;
        _gradingService = gradingService;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<int> Handle(BulkEnterMarksCommand request, CancellationToken cancellationToken)
    {
        // Get component to validate marks
        var component = await _componentRepository.GetByIdAsync(request.AssessmentComponentId, cancellationToken);
        if (component == null)
        {
            throw new InvalidOperationException(
                $"Assessment component with ID {request.AssessmentComponentId} not found.");
        }

        var entriesToAdd = new List<MarkEntry>();
        var entriesToUpdate = new List<MarkEntry>();
        var processedCount = 0;

        foreach (var studentMark in request.StudentMarks)
        {
            try
            {
                // Check if entry exists
                var existingEntry = await _markEntryRepository.GetByComponentAndStudentAsync(
                    request.AssessmentComponentId,
                    studentMark.StudentId,
                    cancellationToken);

                if (existingEntry != null)
                {
                    // Update existing
                    existingEntry.UpdateMarks(
                        studentMark.MarksObtained,
                        studentMark.IsAbsent,
                        studentMark.IsExempted,
                        studentMark.Remarks,
                        request.EnteredBy);

                    existingEntry.CalculatePercentage(component.MaxMarks);
                    var percentage = existingEntry.Percentage ?? 0;
                    var grade = _gradingService.CalculateGrade(percentage);
                    existingEntry.AssignGrade(grade, request.EnteredBy);

                    entriesToUpdate.Add(existingEntry);
                }
                else
                {
                    // Create new
                    var markEntry = new MarkEntry(
                        request.AssessmentComponentId,
                        studentMark.StudentId,
                        studentMark.MarksObtained,
                        studentMark.IsAbsent,
                        studentMark.IsExempted,
                        studentMark.Remarks,
                        request.EnteredBy);

                    markEntry.CalculatePercentage(component.MaxMarks);
                    var percentage = markEntry.Percentage ?? 0;
                    var grade = _gradingService.CalculateGrade(percentage);
                    markEntry.AssignGrade(grade, request.EnteredBy);

                    entriesToAdd.Add(markEntry);
                }

                processedCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error processing marks for student {StudentId} in component {ComponentId}",
                    studentMark.StudentId,
                    request.AssessmentComponentId);
                // Continue with next student
            }
        }

        // Bulk save
        if (entriesToAdd.Any())
        {
            await _markEntryRepository.AddRangeAsync(entriesToAdd, cancellationToken);
        }

        if (entriesToUpdate.Any())
        {
            await _markEntryRepository.UpdateRangeAsync(entriesToUpdate, cancellationToken);
        }

        _logger.LogInformation(
            "Bulk entered marks for {Count} students in component {ComponentId}",
            processedCount,
            request.AssessmentComponentId);

        return processedCount;
    }
}






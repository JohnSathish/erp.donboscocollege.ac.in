using ERP.Application.Examinations.Interfaces;
using ERP.Domain.Examinations.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Examinations.Commands.EnterMarks;

public class EnterMarksCommandHandler : IRequestHandler<EnterMarksCommand, Guid>
{
    private readonly IMarkEntryRepository _markEntryRepository;
    private readonly IAssessmentComponentRepository _componentRepository;
    private readonly IGradingService _gradingService;
    private readonly ILogger<EnterMarksCommandHandler> _logger;

    public EnterMarksCommandHandler(
        IMarkEntryRepository markEntryRepository,
        IAssessmentComponentRepository componentRepository,
        IGradingService gradingService,
        ILogger<EnterMarksCommandHandler> logger)
    {
        _markEntryRepository = markEntryRepository;
        _componentRepository = componentRepository;
        _gradingService = gradingService;
        _logger = logger;
    }

    public async Task<Guid> Handle(EnterMarksCommand request, CancellationToken cancellationToken)
    {
        // Get component to validate marks
        var component = await _componentRepository.GetByIdAsync(request.AssessmentComponentId, cancellationToken);
        if (component == null)
        {
            throw new InvalidOperationException(
                $"Assessment component with ID {request.AssessmentComponentId} not found.");
        }

        // Validate marks
        if (!request.IsAbsent && !request.IsExempted && request.MarksObtained > component.MaxMarks)
        {
            throw new ArgumentException(
                $"Marks obtained ({request.MarksObtained}) cannot exceed maximum marks ({component.MaxMarks}).",
                nameof(request.MarksObtained));
        }

        // Check if mark entry already exists
        var existingEntry = await _markEntryRepository.GetByComponentAndStudentAsync(
            request.AssessmentComponentId,
            request.StudentId,
            cancellationToken);

        MarkEntry markEntry;

        if (existingEntry != null)
        {
            // Update existing entry
            existingEntry.UpdateMarks(
                request.MarksObtained,
                request.IsAbsent,
                request.IsExempted,
                request.Remarks,
                request.EnteredBy);

            // Calculate percentage and grade
            existingEntry.CalculatePercentage(component.MaxMarks);
            var percentage = existingEntry.Percentage ?? 0;
            var grade = _gradingService.CalculateGrade(percentage);
            existingEntry.AssignGrade(grade, request.EnteredBy);

            await _markEntryRepository.UpdateAsync(existingEntry, cancellationToken);
            markEntry = existingEntry;
        }
        else
        {
            // Create new entry
            markEntry = new MarkEntry(
                request.AssessmentComponentId,
                request.StudentId,
                request.MarksObtained,
                request.IsAbsent,
                request.IsExempted,
                request.Remarks,
                request.EnteredBy);

            // Calculate percentage and grade
            markEntry.CalculatePercentage(component.MaxMarks);
            var percentage = markEntry.Percentage ?? 0;
            var grade = _gradingService.CalculateGrade(percentage);
            markEntry.AssignGrade(grade, request.EnteredBy);

            await _markEntryRepository.AddAsync(markEntry, cancellationToken);
        }

        _logger.LogInformation(
            "Entered marks {Marks} for student {StudentId} in component {ComponentId}",
            request.MarksObtained,
            request.StudentId,
            request.AssessmentComponentId);

        return markEntry.Id;
    }
}






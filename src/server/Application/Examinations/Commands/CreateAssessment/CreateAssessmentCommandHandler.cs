using ERP.Application.Examinations.Interfaces;
using ERP.Domain.Examinations.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Examinations.Commands.CreateAssessment;

public class CreateAssessmentCommandHandler : IRequestHandler<CreateAssessmentCommand, Guid>
{
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IAssessmentComponentRepository _componentRepository;
    private readonly ILogger<CreateAssessmentCommandHandler> _logger;

    public CreateAssessmentCommandHandler(
        IAssessmentRepository assessmentRepository,
        IAssessmentComponentRepository componentRepository,
        ILogger<CreateAssessmentCommandHandler> logger)
    {
        _assessmentRepository = assessmentRepository;
        _componentRepository = componentRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateAssessmentCommand request, CancellationToken cancellationToken)
    {
        // Check if code already exists
        var codeExists = await _assessmentRepository.CodeExistsAsync(
            request.Code,
            request.CourseId,
            request.AcademicTermId,
            null,
            cancellationToken);

        if (codeExists)
        {
            throw new InvalidOperationException(
                $"Assessment with code '{request.Code}' already exists for this course and term.");
        }

        // Parse assessment type
        if (!Enum.TryParse<AssessmentType>(request.Type, true, out var assessmentType))
        {
            throw new ArgumentException($"Invalid assessment type: {request.Type}", nameof(request.Type));
        }

        // Create assessment
        var assessment = new Assessment(
            request.CourseId,
            request.AcademicTermId,
            request.Name,
            request.Code,
            assessmentType,
            request.MaxMarks,
            request.PassingMarks,
            request.TotalWeightage,
            request.ClassSectionId,
            request.ScheduledDate,
            request.Duration,
            request.Instructions,
            request.CreatedBy);

        await _assessmentRepository.AddAsync(assessment, cancellationToken);

        // Create components if provided
        if (request.Components != null && request.Components.Any())
        {
            var totalComponentWeightage = request.Components.Sum(c => c.Weightage);
            if (Math.Abs(totalComponentWeightage - request.TotalWeightage) > 0.01m)
            {
                _logger.LogWarning(
                    "Component weightage total ({Total}) does not match assessment weightage ({AssessmentWeightage})",
                    totalComponentWeightage,
                    request.TotalWeightage);
            }

            foreach (var componentDto in request.Components.OrderBy(c => c.DisplayOrder))
            {
                var component = new AssessmentComponent(
                    assessment.Id,
                    componentDto.Name,
                    componentDto.MaxMarks,
                    componentDto.PassingMarks,
                    componentDto.Weightage,
                    componentDto.DisplayOrder,
                    componentDto.Code,
                    componentDto.Instructions,
                    request.CreatedBy);

                await _componentRepository.AddAsync(component, cancellationToken);
            }
        }

        _logger.LogInformation(
            "Created assessment {AssessmentId} ({Code}) for course {CourseId}",
            assessment.Id,
            assessment.Code,
            request.CourseId);

        return assessment.Id;
    }
}






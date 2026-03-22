using ERP.Application.Examinations.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Examinations.Commands.PublishAssessment;

public class PublishAssessmentCommandHandler : IRequestHandler<PublishAssessmentCommand, Unit>
{
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly ILogger<PublishAssessmentCommandHandler> _logger;

    public PublishAssessmentCommandHandler(
        IAssessmentRepository assessmentRepository,
        ILogger<PublishAssessmentCommandHandler> logger)
    {
        _assessmentRepository = assessmentRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(PublishAssessmentCommand request, CancellationToken cancellationToken)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(request.AssessmentId, cancellationToken);
        if (assessment == null)
        {
            throw new InvalidOperationException($"Assessment with ID {request.AssessmentId} not found.");
        }

        assessment.Publish(request.PublishedBy);
        await _assessmentRepository.UpdateAsync(assessment, cancellationToken);

        _logger.LogInformation(
            "Published assessment {AssessmentId} ({Code}) by {PublishedBy}",
            request.AssessmentId,
            assessment.Code,
            request.PublishedBy);

        return Unit.Value;
    }
}


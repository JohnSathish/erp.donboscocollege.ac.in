using MediatR;

namespace ERP.Application.Examinations.Commands.PublishAssessment;

public record PublishAssessmentCommand(
    Guid AssessmentId,
    string? PublishedBy = null) : IRequest<Unit>;


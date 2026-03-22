using MediatR;

namespace ERP.Application.Examinations.Commands.ProcessResults;

public record ProcessResultsCommand(
    Guid StudentId,
    Guid AcademicTermId,
    string? ProcessedBy = null) : IRequest<Guid>;






using ERP.Application.Examinations.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Examinations.Commands.ProcessResults;

public class ProcessResultsCommandHandler : IRequestHandler<ProcessResultsCommand, Guid>
{
    private readonly IResultProcessingService _resultProcessingService;
    private readonly ILogger<ProcessResultsCommandHandler> _logger;

    public ProcessResultsCommandHandler(
        IResultProcessingService resultProcessingService,
        ILogger<ProcessResultsCommandHandler> logger)
    {
        _resultProcessingService = resultProcessingService;
        _logger = logger;
    }

    public async Task<Guid> Handle(ProcessResultsCommand request, CancellationToken cancellationToken)
    {
        var resultSummary = await _resultProcessingService.ProcessStudentResultsAsync(
            request.StudentId,
            request.AcademicTermId,
            cancellationToken);

        _logger.LogInformation(
            "Processed results for student {StudentId} in term {TermId}. Result Summary ID: {ResultSummaryId}",
            request.StudentId,
            request.AcademicTermId,
            resultSummary.Id);

        return resultSummary.Id;
    }
}






using ERP.Application.Examinations.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Examinations.Commands.ApproveMarks;

public class ApproveMarksCommandHandler : IRequestHandler<ApproveMarksCommand, Unit>
{
    private readonly IMarkEntryRepository _markEntryRepository;
    private readonly ILogger<ApproveMarksCommandHandler> _logger;

    public ApproveMarksCommandHandler(
        IMarkEntryRepository markEntryRepository,
        ILogger<ApproveMarksCommandHandler> logger)
    {
        _markEntryRepository = markEntryRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(ApproveMarksCommand request, CancellationToken cancellationToken)
    {
        var markEntry = await _markEntryRepository.GetByIdAsync(request.MarkEntryId, cancellationToken);
        if (markEntry == null)
        {
            throw new InvalidOperationException($"Mark entry with ID {request.MarkEntryId} not found.");
        }

        markEntry.Approve(request.ApprovedBy);
        await _markEntryRepository.UpdateAsync(markEntry, cancellationToken);

        _logger.LogInformation(
            "Approved mark entry {MarkEntryId} by {ApprovedBy}",
            request.MarkEntryId,
            request.ApprovedBy);

        return Unit.Value;
    }
}


using ERP.Application.Academics.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Academics.Commands.DeactivateAcademicTerm;

public sealed class DeactivateAcademicTermCommandHandler : IRequestHandler<DeactivateAcademicTermCommand, bool>
{
    private readonly IAcademicTermRepository _termRepository;
    private readonly ILogger<DeactivateAcademicTermCommandHandler> _logger;

    public DeactivateAcademicTermCommandHandler(
        IAcademicTermRepository termRepository,
        ILogger<DeactivateAcademicTermCommandHandler> logger)
    {
        _termRepository = termRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeactivateAcademicTermCommand request, CancellationToken cancellationToken)
    {
        var term = await _termRepository.GetByIdForUpdateAsync(request.TermId, cancellationToken);
        if (term == null)
        {
            _logger.LogWarning("Academic term {TermId} not found for deactivation.", request.TermId);
            return false;
        }

        term.Deactivate(request.UpdatedBy);
        await _termRepository.UpdateAsync(term, cancellationToken);
        await _termRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Academic term {TermId} deactivated.", request.TermId);
        return true;
    }
}


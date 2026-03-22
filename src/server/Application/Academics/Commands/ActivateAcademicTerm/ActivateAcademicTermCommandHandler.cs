using ERP.Application.Academics.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Academics.Commands.ActivateAcademicTerm;

public sealed class ActivateAcademicTermCommandHandler : IRequestHandler<ActivateAcademicTermCommand, bool>
{
    private readonly IAcademicTermRepository _termRepository;
    private readonly ILogger<ActivateAcademicTermCommandHandler> _logger;

    public ActivateAcademicTermCommandHandler(
        IAcademicTermRepository termRepository,
        ILogger<ActivateAcademicTermCommandHandler> logger)
    {
        _termRepository = termRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(ActivateAcademicTermCommand request, CancellationToken cancellationToken)
    {
        var term = await _termRepository.GetByIdForUpdateAsync(request.TermId, cancellationToken);
        if (term == null)
        {
            _logger.LogWarning("Academic term {TermId} not found for activation.", request.TermId);
            return false;
        }

        term.Activate(request.UpdatedBy);
        await _termRepository.UpdateAsync(term, cancellationToken);
        await _termRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Academic term {TermId} activated.", request.TermId);
        return true;
    }
}


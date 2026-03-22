using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.AssignSelectionListRound;

public sealed class AssignSelectionListRoundCommandHandler
    : IRequestHandler<AssignSelectionListRoundCommand, AssignSelectionListRoundResult>
{
    private readonly IApplicantAccountRepository _repository;

    public AssignSelectionListRoundCommandHandler(IApplicantAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<AssignSelectionListRoundResult> Handle(
        AssignSelectionListRoundCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _repository.GetByIdForUpdateAsync(request.ApplicationId, cancellationToken)
                      ?? throw new InvalidOperationException("Application not found.");

        if (!account.IsApplicationSubmitted)
        {
            throw new InvalidOperationException("Application must be submitted before assigning a selection list round.");
        }

        account.AssignSelectionListRound(request.Round);
        await _repository.UpdateAsync(account, cancellationToken);

        return new AssignSelectionListRoundResult(account.Id, account.UniqueId, request.Round);
    }
}

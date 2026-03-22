using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.EnterExamScore;

public sealed class EnterExamScoreCommandHandler : IRequestHandler<EnterExamScoreCommand, bool>
{
    private readonly IExamRegistrationRepository _registrationRepository;

    public EnterExamScoreCommandHandler(IExamRegistrationRepository registrationRepository)
    {
        _registrationRepository = registrationRepository;
    }

    public async Task<bool> Handle(EnterExamScoreCommand request, CancellationToken cancellationToken)
    {
        var registration = await _registrationRepository.GetByIdAsync(request.RegistrationId, cancellationToken);
        if (registration is null)
        {
            return false;
        }

        registration.EnterScore(request.Score, request.EnteredBy);
        await _registrationRepository.UpdateAsync(registration, cancellationToken);

        return true;
    }
}












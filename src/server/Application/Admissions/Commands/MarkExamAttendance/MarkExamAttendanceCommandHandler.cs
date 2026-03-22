using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.MarkExamAttendance;

public sealed class MarkExamAttendanceCommandHandler : IRequestHandler<MarkExamAttendanceCommand, bool>
{
    private readonly IExamRegistrationRepository _registrationRepository;

    public MarkExamAttendanceCommandHandler(IExamRegistrationRepository registrationRepository)
    {
        _registrationRepository = registrationRepository;
    }

    public async Task<bool> Handle(MarkExamAttendanceCommand request, CancellationToken cancellationToken)
    {
        var registration = await _registrationRepository.GetByIdAsync(request.RegistrationId, cancellationToken);
        if (registration is null)
        {
            return false;
        }

        registration.MarkAttendance(request.IsPresent, request.MarkedBy);
        await _registrationRepository.UpdateAsync(registration, cancellationToken);

        return true;
    }
}












using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.CreateApplicant;

public sealed class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, Guid>
{
    private readonly IAdmissionsRepository _repository;

    public CreateApplicantCommandHandler(IAdmissionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        var applicant = new Applicant(
            request.ApplicationNumber,
            request.FirstName,
            request.LastName,
            request.Email,
            request.DateOfBirth,
            request.ProgramCode,
            request.MobileNumber);

        var created = await _repository.AddApplicantAsync(applicant, cancellationToken);

        return created.Id;
    }
}



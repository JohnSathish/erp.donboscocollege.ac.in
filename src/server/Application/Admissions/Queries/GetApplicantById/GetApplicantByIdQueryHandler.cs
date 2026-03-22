using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetApplicantById;

public sealed class GetApplicantByIdQueryHandler : IRequestHandler<GetApplicantByIdQuery, ApplicantDto?>
{
    private readonly IAdmissionsReadRepository _repository;

    public GetApplicantByIdQueryHandler(IAdmissionsReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApplicantDto?> Handle(GetApplicantByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetApplicantByIdAsync(request.ApplicantId, cancellationToken);
    }
}



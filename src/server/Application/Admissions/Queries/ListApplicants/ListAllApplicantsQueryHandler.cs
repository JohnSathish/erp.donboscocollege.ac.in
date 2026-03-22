using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListApplicants;

public sealed class ListAllApplicantsQueryHandler : IRequestHandler<ListAllApplicantsQuery, IReadOnlyCollection<ApplicantDto>>
{
    private readonly IAdmissionsReadRepository _repository;

    public ListAllApplicantsQueryHandler(IAdmissionsReadRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<ApplicantDto>> Handle(ListAllApplicantsQuery request, CancellationToken cancellationToken)
    {
        return _repository.ListAllApplicantsAsync(cancellationToken);
    }
}


using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListApplicants;

public sealed class ListApplicantsQueryHandler : IRequestHandler<ListApplicantsQuery, IReadOnlyCollection<ApplicantDto>>
{
    private readonly IAdmissionsReadRepository _repository;

    public ListApplicantsQueryHandler(IAdmissionsReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<ApplicantDto>> Handle(ListApplicantsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var size = Math.Clamp(request.PageSize, 1, 200);

        return await _repository.ListApplicantsAsync(page, size, cancellationToken);
    }
}



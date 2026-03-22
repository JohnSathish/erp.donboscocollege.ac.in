using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListApplicants;

public sealed record ListApplicantsQuery(int Page = 1, int PageSize = 50) : IRequest<IReadOnlyCollection<ApplicantDto>>;

public sealed record ListAllApplicantsQuery() : IRequest<IReadOnlyCollection<ApplicantDto>>;



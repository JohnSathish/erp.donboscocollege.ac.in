using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetApplicantDashboard;

public sealed record GetApplicantDashboardQuery(Guid AccountId) : IRequest<ApplicantDashboardDto?>;








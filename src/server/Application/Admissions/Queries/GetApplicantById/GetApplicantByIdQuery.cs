using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetApplicantById;

public sealed record GetApplicantByIdQuery(Guid ApplicantId) : IRequest<ApplicantDto?>;



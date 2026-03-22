using ERP.Application.Admissions.DTOs;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetApplicantApplicationDraft;

public sealed record GetApplicantApplicationDraftQuery(Guid AccountId) : IRequest<ApplicantApplicationDraftResponse>;






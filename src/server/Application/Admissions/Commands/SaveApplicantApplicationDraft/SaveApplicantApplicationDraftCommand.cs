using ERP.Application.Admissions.DTOs;
using MediatR;

namespace ERP.Application.Admissions.Commands.SaveApplicantApplicationDraft;

public sealed record SaveApplicantApplicationDraftCommand(Guid AccountId, ApplicantApplicationDraftDto Payload) : IRequest<ApplicantApplicationDraftResponse>;






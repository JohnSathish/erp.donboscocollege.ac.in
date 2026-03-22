using ERP.Application.Admissions.DTOs;
using MediatR;

namespace ERP.Application.Admissions.Commands.SubmitApplicantApplication;

public sealed record SubmitApplicantApplicationCommand(
    Guid AccountId,
    ApplicantApplicationDraftDto Payload) : IRequest<ApplicantApplicationPdfResult>;






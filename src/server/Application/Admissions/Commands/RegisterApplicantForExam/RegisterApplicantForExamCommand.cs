using MediatR;

namespace ERP.Application.Admissions.Commands.RegisterApplicantForExam;

public sealed record RegisterApplicantForExamCommand(
    Guid ExamId,
    Guid ApplicantAccountId,
    string? RegisteredBy = null) : IRequest<Guid>;














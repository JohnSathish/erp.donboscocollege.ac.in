using MediatR;

namespace ERP.Application.Admissions.Commands.EnterExamScore;

public sealed record EnterExamScoreCommand(
    Guid RegistrationId,
    decimal Score,
    string? EnteredBy = null) : IRequest<bool>;












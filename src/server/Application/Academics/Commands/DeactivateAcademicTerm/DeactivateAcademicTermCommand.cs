using MediatR;

namespace ERP.Application.Academics.Commands.DeactivateAcademicTerm;

public sealed record DeactivateAcademicTermCommand(
    Guid TermId,
    string? UpdatedBy = null) : IRequest<bool>;


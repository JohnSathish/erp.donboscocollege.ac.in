using MediatR;

namespace ERP.Application.Academics.Commands.ActivateAcademicTerm;

public sealed record ActivateAcademicTermCommand(
    Guid TermId,
    string? UpdatedBy = null) : IRequest<bool>;


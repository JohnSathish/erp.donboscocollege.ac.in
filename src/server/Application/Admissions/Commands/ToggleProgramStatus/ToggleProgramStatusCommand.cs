using MediatR;

namespace ERP.Application.Admissions.Commands.ToggleProgramStatus;

public sealed record ToggleProgramStatusCommand(Guid Id, string? UpdatedBy = null) : IRequest<bool>;










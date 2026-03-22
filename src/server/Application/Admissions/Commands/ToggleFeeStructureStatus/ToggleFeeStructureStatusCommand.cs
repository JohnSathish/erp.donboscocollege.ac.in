using MediatR;

namespace ERP.Application.Admissions.Commands.ToggleFeeStructureStatus;

public sealed record ToggleFeeStructureStatusCommand(Guid Id, string? UpdatedBy = null) : IRequest<bool>;



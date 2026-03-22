using MediatR;

namespace ERP.Application.Admissions.Commands.ToggleCourseStatus;

public sealed record ToggleCourseStatusCommand(Guid Id, string? UpdatedBy = null) : IRequest<bool>;










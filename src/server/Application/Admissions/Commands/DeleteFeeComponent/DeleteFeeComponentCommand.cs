using MediatR;

namespace ERP.Application.Admissions.Commands.DeleteFeeComponent;

public sealed record DeleteFeeComponentCommand(Guid Id) : IRequest<bool>;




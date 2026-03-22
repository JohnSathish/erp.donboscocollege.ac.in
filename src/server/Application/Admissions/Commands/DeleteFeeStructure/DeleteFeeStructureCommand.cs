using MediatR;

namespace ERP.Application.Admissions.Commands.DeleteFeeStructure;

public sealed record DeleteFeeStructureCommand(Guid Id) : IRequest<bool>;




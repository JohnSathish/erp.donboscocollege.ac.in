using MediatR;

namespace ERP.Application.Admissions.Commands.DeleteProgram;

public sealed record DeleteProgramCommand(Guid Id) : IRequest<bool>;










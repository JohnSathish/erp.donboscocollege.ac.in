using MediatR;

namespace ERP.Application.Admissions.Commands.CreateProgram;

public sealed record CreateProgramCommand(
    string Code,
    string Name,
    string Level,
    int DurationYears,
    int TotalCredits,
    string? Description = null,
    string? CreatedBy = null) : IRequest<Guid>;










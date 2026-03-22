using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateProgram;

public sealed record UpdateProgramCommand(
    Guid Id,
    string Name,
    string Level,
    int DurationYears,
    int TotalCredits,
    string? Description = null,
    string? UpdatedBy = null) : IRequest<bool>;










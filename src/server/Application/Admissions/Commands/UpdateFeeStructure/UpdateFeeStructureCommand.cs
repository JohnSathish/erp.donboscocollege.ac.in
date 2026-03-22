using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateFeeStructure;

public sealed record UpdateFeeStructureCommand(
    Guid Id,
    string Name,
    string AcademicYear,
    DateTime EffectiveFromUtc,
    Guid? ProgramId = null,
    string? Description = null,
    DateTime? EffectiveToUtc = null,
    string? UpdatedBy = null) : IRequest<bool>;










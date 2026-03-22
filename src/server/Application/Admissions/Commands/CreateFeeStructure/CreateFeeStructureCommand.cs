using MediatR;

namespace ERP.Application.Admissions.Commands.CreateFeeStructure;

public sealed record CreateFeeStructureCommand(
    string Name,
    string AcademicYear,
    DateTime EffectiveFromUtc,
    Guid? ProgramId = null,
    string? Description = null,
    DateTime? EffectiveToUtc = null,
    IReadOnlyCollection<FeeComponentRequest>? Components = null,
    string? CreatedBy = null) : IRequest<Guid>;

public sealed record FeeComponentRequest(
    string Name,
    decimal Amount,
    bool IsOptional = false,
    int? InstallmentNumber = null,
    DateTime? DueDateUtc = null,
    string? Description = null,
    int DisplayOrder = 0);










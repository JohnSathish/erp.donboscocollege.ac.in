using MediatR;

namespace ERP.Application.Admissions.Commands.AddFeeComponent;

public sealed record AddFeeComponentCommand(
    Guid FeeStructureId,
    string Name,
    decimal Amount,
    bool IsOptional = false,
    int? InstallmentNumber = null,
    DateTime? DueDateUtc = null,
    string? Description = null,
    int DisplayOrder = 0,
    string? CreatedBy = null) : IRequest<Guid>;










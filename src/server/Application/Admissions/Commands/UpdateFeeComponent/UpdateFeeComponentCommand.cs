using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateFeeComponent;

public sealed record UpdateFeeComponentCommand(
    Guid Id,
    string Name,
    decimal Amount,
    bool IsOptional = false,
    int? InstallmentNumber = null,
    DateTime? DueDateUtc = null,
    string? Description = null,
    int DisplayOrder = 0,
    string? UpdatedBy = null) : IRequest<bool>;










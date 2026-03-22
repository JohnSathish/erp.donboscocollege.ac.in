using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Students.Commands.TransferStudent;

public sealed record TransferStudentCommand(
    Guid StudentId,
    [Required] string Reason,
    DateTime EffectiveDate,
    Guid? ToProgramId = null,
    string? ToProgramCode = null,
    string? ToShift = null,
    string? ToSection = null,
    string? RequestedBy = null,
    string? Remarks = null) : IRequest<TransferStudentResult>;

public sealed record TransferStudentResult(
    Guid TransferId,
    Guid StudentId,
    string StudentNumber,
    bool Success,
    string? ErrorMessage = null);


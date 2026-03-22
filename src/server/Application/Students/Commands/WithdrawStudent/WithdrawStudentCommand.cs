using MediatR;
using ERP.Domain.Students.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Students.Commands.WithdrawStudent;

public sealed record WithdrawStudentCommand(
    Guid StudentId,
    [Required] ExitType ExitType,
    [Required] string Reason,
    DateTime? EffectiveDate = null,
    string? RequestedBy = null,
    string? Remarks = null) : IRequest<WithdrawStudentResult>;

public sealed record WithdrawStudentResult(
    Guid ExitId,
    Guid StudentId,
    string StudentNumber,
    bool Success,
    string? ErrorMessage = null);


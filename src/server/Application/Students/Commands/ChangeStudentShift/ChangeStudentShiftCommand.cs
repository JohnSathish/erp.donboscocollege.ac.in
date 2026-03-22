using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Students.Commands.ChangeStudentShift;

public sealed record ChangeStudentShiftCommand(
    Guid StudentId,
    [Required] string NewShift,
    string? UpdatedBy = null) : IRequest<ChangeStudentShiftResult>;

public sealed record ChangeStudentShiftResult(
    Guid StudentId,
    string StudentNumber,
    string OldShift,
    string NewShift,
    bool Success,
    string? ErrorMessage = null);


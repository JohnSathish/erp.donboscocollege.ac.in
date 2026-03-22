using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Students.Commands.CreateCounselingRecord;

public sealed record CreateCounselingRecordCommand(
    Guid StudentId,
    [Required] string SessionType,
    [Required] DateTime SessionDate,
    string? CounselorName = null,
    string? CounselorId = null,
    string? Location = null,
    string? CreatedBy = null) : IRequest<Guid>;


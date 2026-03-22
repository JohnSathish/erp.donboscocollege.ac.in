using MediatR;

namespace ERP.Application.Examinations.Commands.ApproveMarks;

public record ApproveMarksCommand(
    Guid MarkEntryId,
    string? ApprovedBy = null) : IRequest<Unit>;


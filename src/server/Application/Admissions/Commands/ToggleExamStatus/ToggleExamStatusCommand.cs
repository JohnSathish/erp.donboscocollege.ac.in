using MediatR;

namespace ERP.Application.Admissions.Commands.ToggleExamStatus;

public sealed record ToggleExamStatusCommand(
    Guid ExamId,
    bool IsActive,
    string? UpdatedBy = null) : IRequest<bool>;












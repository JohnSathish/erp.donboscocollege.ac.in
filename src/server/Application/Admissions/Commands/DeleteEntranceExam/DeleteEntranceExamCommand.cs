using MediatR;

namespace ERP.Application.Admissions.Commands.DeleteEntranceExam;

public sealed record DeleteEntranceExamCommand(
    Guid ExamId,
    string? DeletedBy = null) : IRequest<bool>;












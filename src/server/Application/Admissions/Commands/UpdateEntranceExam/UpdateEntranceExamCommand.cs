using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateEntranceExam;

public sealed record UpdateEntranceExamCommand(
    Guid ExamId,
    string ExamName,
    DateTime ExamDate,
    TimeOnly ExamStartTime,
    TimeOnly ExamEndTime,
    string Venue,
    int MaxCapacity,
    string? Description = null,
    string? VenueAddress = null,
    string? Instructions = null,
    string? UpdatedBy = null) : IRequest;














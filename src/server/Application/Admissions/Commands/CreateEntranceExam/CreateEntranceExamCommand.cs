using MediatR;

namespace ERP.Application.Admissions.Commands.CreateEntranceExam;

public sealed record CreateEntranceExamCommand(
    string ExamName,
    string ExamCode,
    DateTime ExamDate,
    TimeOnly ExamStartTime,
    TimeOnly ExamEndTime,
    string Venue,
    int MaxCapacity,
    string? Description = null,
    string? VenueAddress = null,
    string? Instructions = null,
    string? CreatedBy = null) : IRequest<Guid>;














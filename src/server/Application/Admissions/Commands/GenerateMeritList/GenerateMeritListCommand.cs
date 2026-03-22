using MediatR;

namespace ERP.Application.Admissions.Commands.GenerateMeritList;

public sealed record GenerateMeritListCommand(
    string? Shift = null,
    string? MajorSubject = null,
    string? CreatedBy = null) : IRequest<GenerateMeritListResult>;

public sealed record GenerateMeritListResult(
    int TotalApplicantsProcessed,
    int MeritScoresCreated,
    DateTime GeneratedOnUtc);


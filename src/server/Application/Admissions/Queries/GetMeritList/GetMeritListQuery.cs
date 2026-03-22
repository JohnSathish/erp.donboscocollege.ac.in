using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetMeritList;

public sealed record GetMeritListQuery(
    string? Shift = null,
    string? MajorSubject = null,
    int Page = 1,
    int PageSize = 50) : IRequest<MeritListResponse>;

public sealed record MeritListResponse(
    IReadOnlyCollection<MeritScoreDto> MeritScores,
    int TotalCount,
    int Page,
    int PageSize);

public sealed record MeritScoreDto(
    Guid Id,
    Guid AccountId,
    string ApplicationNumber,
    string FullName,
    decimal ClassXIIPercentage,
    decimal? CuetScore,
    decimal? EntranceExamScore,
    decimal TotalScore,
    int Rank,
    string Shift,
    string MajorSubject,
    DateTime CalculatedOnUtc);


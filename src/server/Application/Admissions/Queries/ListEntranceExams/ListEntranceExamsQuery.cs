using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListEntranceExams;

public sealed record ListEntranceExamsQuery(
    int Page,
    int PageSize,
    bool? IsActive = null,
    DateTime? ExamDateFrom = null,
    DateTime? ExamDateTo = null,
    string? SearchTerm = null) : IRequest<EntranceExamsListResponse>;

public sealed record EntranceExamsListResponse(
    IReadOnlyCollection<EntranceExamDto> Exams,
    int TotalCount,
    int Page,
    int PageSize);














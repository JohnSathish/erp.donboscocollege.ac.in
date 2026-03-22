using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListExamRegistrations;

public sealed record ListExamRegistrationsQuery(
    Guid ExamId,
    int Page,
    int PageSize,
    bool? IsPresent = null,
    string? SearchTerm = null) : IRequest<ExamRegistrationsListResponse>;

public sealed record ExamRegistrationsListResponse(
    IReadOnlyCollection<ExamRegistrationDto> Registrations,
    int TotalCount,
    int Page,
    int PageSize);














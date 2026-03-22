using MediatR;

namespace ERP.Application.Admissions.Queries.ListAdmittedStudents;

public sealed record ListAdmittedStudentsQuery(
    int Page = 1,
    int PageSize = 50,
    string? SearchTerm = null
) : IRequest<AdmittedStudentsListResponse>;

public sealed record AdmittedStudentRowDto(
    Guid Id,
    string UniqueId,
    string FullName,
    string? MajorSubject,
    bool IsPaymentCompleted,
    string Status,
    DateTime? PaymentCompletedOnUtc,
    DateTime CreatedOnUtc);

public sealed record AdmittedStudentsListResponse(
    IReadOnlyCollection<AdmittedStudentRowDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

using MediatR;

namespace ERP.Application.Academics.Queries.ListAcademicTerms;

public sealed record ListAcademicTermsQuery(
    string? AcademicYear = null,
    bool? IsActive = null) : IRequest<IReadOnlyCollection<AcademicTermDto>>;

public sealed record AcademicTermDto(
    Guid Id,
    string TermName,
    string AcademicYear,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    string? Remarks,
    DateTime CreatedOnUtc,
    string? CreatedBy);


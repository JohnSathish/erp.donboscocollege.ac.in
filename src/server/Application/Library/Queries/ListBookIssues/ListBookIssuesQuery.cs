using MediatR;
using ERP.Domain.Library.Entities;

namespace ERP.Application.Library.Queries.ListBookIssues;

public sealed record ListBookIssuesQuery(
    int Page = 1,
    int PageSize = 50,
    Guid? BookId = null,
    Guid? StudentId = null,
    Guid? StaffId = null,
    IssueStatus? Status = null) : IRequest<ListBookIssuesResult>;

public sealed record ListBookIssuesResult(
    IReadOnlyCollection<BookIssueDto> Issues,
    int TotalCount,
    int Page,
    int PageSize);

public sealed record BookIssueDto(
    Guid Id,
    Guid BookId,
    string BookTitle,
    string BookIsbn,
    Guid? StudentId,
    string? StudentName,
    Guid? StaffId,
    string? StaffName,
    string IssuedToType,
    DateTime IssueDate,
    DateTime DueDate,
    DateTime? ReturnDate,
    string Status,
    decimal? FineAmount,
    string? Remarks,
    string? IssuedBy,
    string? ReturnedBy,
    DateTime CreatedOnUtc);





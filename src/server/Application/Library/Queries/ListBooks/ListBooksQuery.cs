using MediatR;
using ERP.Domain.Library.Entities;

namespace ERP.Application.Library.Queries.ListBooks;

public sealed record ListBooksQuery(
    int Page = 1,
    int PageSize = 50,
    string? Category = null,
    string? Author = null,
    BookStatus? Status = null,
    string? SearchTerm = null) : IRequest<ListBooksResult>;

public sealed record ListBooksResult(
    IReadOnlyCollection<BookDto> Books,
    int TotalCount,
    int Page,
    int PageSize);

public sealed record BookDto(
    Guid Id,
    string Isbn,
    string Title,
    string Author,
    string? Publisher,
    int? PublicationYear,
    string? Category,
    string? Language,
    int TotalCopies,
    int AvailableCopies,
    decimal? Price,
    string? Location,
    string? Description,
    string Status,
    DateTime CreatedOnUtc,
    string? CreatedBy);





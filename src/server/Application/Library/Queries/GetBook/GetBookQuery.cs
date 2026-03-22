using MediatR;

namespace ERP.Application.Library.Queries.GetBook;

public sealed record GetBookQuery(Guid BookId) : IRequest<GetBookResult?>;

public sealed record GetBookResult(
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
    string? CreatedBy,
    DateTime? UpdatedOnUtc,
    string? UpdatedBy);


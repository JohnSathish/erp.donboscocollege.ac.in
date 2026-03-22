using MediatR;

namespace ERP.Application.Library.Commands.CreateBook;

public sealed record CreateBookCommand(
    string Isbn,
    string Title,
    string Author,
    int TotalCopies,
    string? Publisher = null,
    int? PublicationYear = null,
    string? Category = null,
    string? Language = null,
    decimal? Price = null,
    string? Location = null,
    string? Description = null,
    string? CreatedBy = null) : IRequest<Guid>;





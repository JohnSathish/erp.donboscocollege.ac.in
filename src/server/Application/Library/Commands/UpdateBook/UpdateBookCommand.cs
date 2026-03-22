using MediatR;

namespace ERP.Application.Library.Commands.UpdateBook;

public sealed record UpdateBookCommand(
    Guid BookId,
    string? Title = null,
    string? Author = null,
    string? Publisher = null,
    int? PublicationYear = null,
    string? Category = null,
    string? Language = null,
    int? TotalCopies = null,
    decimal? Price = null,
    string? Location = null,
    string? Description = null,
    string? UpdatedBy = null) : IRequest;


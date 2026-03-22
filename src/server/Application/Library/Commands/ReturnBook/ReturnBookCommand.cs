using MediatR;

namespace ERP.Application.Library.Commands.ReturnBook;

public sealed record ReturnBookCommand(
    Guid IssueId,
    DateTime ReturnDate,
    decimal? FineAmount = null,
    string? ReturnedBy = null,
    string? Remarks = null) : IRequest<Unit>;





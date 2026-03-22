using ERP.Application.Library.Interfaces;
using MediatR;

namespace ERP.Application.Library.Queries.ListBooks;

public sealed class ListBooksQueryHandler : IRequestHandler<ListBooksQuery, ListBooksResult>
{
    private readonly IBookRepository _bookRepository;

    public ListBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<ListBooksResult> Handle(ListBooksQuery request, CancellationToken cancellationToken)
    {
        var (books, totalCount) = await _bookRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Category,
            request.Author,
            request.Status,
            request.SearchTerm,
            cancellationToken);

        var bookDtos = books.Select(b => new BookDto(
            b.Id,
            b.Isbn,
            b.Title,
            b.Author,
            b.Publisher,
            b.PublicationYear,
            b.Category,
            b.Language,
            b.TotalCopies,
            b.AvailableCopies,
            b.Price,
            b.Location,
            b.Description,
            b.Status.ToString(),
            b.CreatedOnUtc,
            b.CreatedBy)).ToList();

        return new ListBooksResult(
            bookDtos,
            totalCount,
            request.Page,
            request.PageSize);
    }
}





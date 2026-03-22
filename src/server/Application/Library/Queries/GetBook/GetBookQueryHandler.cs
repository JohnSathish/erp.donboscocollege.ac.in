using ERP.Application.Library.Interfaces;
using MediatR;

namespace ERP.Application.Library.Queries.GetBook;

public sealed class GetBookQueryHandler : IRequestHandler<GetBookQuery, GetBookResult?>
{
    private readonly IBookRepository _repository;

    public GetBookQueryHandler(IBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetBookResult?> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync(request.BookId, cancellationToken);
        
        if (book == null)
        {
            return null;
        }

        return new GetBookResult(
            book.Id,
            book.Isbn,
            book.Title,
            book.Author,
            book.Publisher,
            book.PublicationYear,
            book.Category,
            book.Language,
            book.TotalCopies,
            book.AvailableCopies,
            book.Price,
            book.Location,
            book.Description,
            book.Status.ToString(),
            book.CreatedOnUtc,
            book.CreatedBy,
            book.UpdatedOnUtc,
            book.UpdatedBy);
    }
}


using ERP.Application.Library.Interfaces;
using ERP.Domain.Library.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Library.Commands.CreateBook;

public sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<CreateBookCommandHandler> _logger;

    public CreateBookCommandHandler(
        IBookRepository bookRepository,
        ILogger<CreateBookCommandHandler> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        // Check if ISBN already exists
        var exists = await _bookRepository.IsbnExistsAsync(request.Isbn, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException(
                $"Book with ISBN '{request.Isbn}' already exists.");
        }

        var book = new Book(
            request.Isbn,
            request.Title,
            request.Author,
            request.TotalCopies,
            request.Publisher,
            request.PublicationYear,
            request.Category,
            request.Language,
            request.Price,
            request.Location,
            request.Description,
            request.CreatedBy);

        await _bookRepository.AddAsync(book, cancellationToken);

        _logger.LogInformation(
            "Created book {Isbn} ({Title})",
            book.Isbn,
            book.Title);

        return book.Id;
    }
}





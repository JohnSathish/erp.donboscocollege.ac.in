using ERP.Application.Library.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Library.Commands.UpdateBook;

public sealed class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
{
    private readonly IBookRepository _repository;
    private readonly ILogger<UpdateBookCommandHandler> _logger;

    public UpdateBookCommandHandler(
        IBookRepository repository,
        ILogger<UpdateBookCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync(request.BookId, cancellationToken);
        
        if (book == null)
        {
            throw new InvalidOperationException($"Book with ID '{request.BookId}' not found.");
        }

        // Update details if provided
        if (request.Title != null || request.Author != null)
        {
            book.UpdateDetails(
                request.Title ?? book.Title,
                request.Author ?? book.Author,
                request.Publisher ?? book.Publisher,
                request.PublicationYear ?? book.PublicationYear,
                request.Category ?? book.Category,
                request.Language ?? book.Language,
                request.Price ?? book.Price,
                request.Location ?? book.Location,
                request.Description ?? book.Description,
                request.UpdatedBy);
        }

        // Update copies if provided
        if (request.TotalCopies.HasValue)
        {
            book.UpdateCopies(request.TotalCopies.Value, request.UpdatedBy);
        }

        await _repository.UpdateAsync(book, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Updated book {BookId} ({Isbn}) by {UpdatedBy}",
            book.Id,
            book.Isbn,
            request.UpdatedBy ?? "System");

        return Unit.Value;
    }
}


using ERP.Application.Library.Interfaces;
using ERP.Domain.Library.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Library.Commands.ReturnBook;

public sealed class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, Unit>
{
    private readonly IBookIssueRepository _issueRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<ReturnBookCommandHandler> _logger;

    public ReturnBookCommandHandler(
        IBookIssueRepository issueRepository,
        IBookRepository bookRepository,
        ILogger<ReturnBookCommandHandler> logger)
    {
        _issueRepository = issueRepository;
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetByIdAsync(request.IssueId, cancellationToken);
        if (issue == null)
        {
            throw new InvalidOperationException($"Book issue with ID '{request.IssueId}' not found.");
        }

        if (issue.Status != IssueStatus.Issued)
        {
            throw new InvalidOperationException($"Book issue is already returned or closed.");
        }

        // Return the book
        issue.Return(request.ReturnDate, request.FineAmount, request.ReturnedBy, request.Remarks);
        await _issueRepository.UpdateAsync(issue, cancellationToken);

        // Increase available copies
        var book = await _bookRepository.GetByIdAsync(issue.BookId, cancellationToken);
        if (book != null)
        {
            book.IncreaseAvailableCopies(1);
            await _bookRepository.UpdateAsync(book, cancellationToken);
        }

        _logger.LogInformation(
            "Returned book issue {IssueId}",
            request.IssueId);

        return Unit.Value;
    }
}


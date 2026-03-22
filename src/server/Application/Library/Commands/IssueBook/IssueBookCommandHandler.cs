using ERP.Application.Library.Interfaces;
using ERP.Domain.Library.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Library.Commands.IssueBook;

public sealed class IssueBookCommandHandler : IRequestHandler<IssueBookCommand, Guid>
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookIssueRepository _issueRepository;
    private readonly ILogger<IssueBookCommandHandler> _logger;

    public IssueBookCommandHandler(
        IBookRepository bookRepository,
        IBookIssueRepository issueRepository,
        ILogger<IssueBookCommandHandler> logger)
    {
        _bookRepository = bookRepository;
        _issueRepository = issueRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(IssueBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);
        if (book == null)
        {
            throw new InvalidOperationException($"Book with ID '{request.BookId}' not found.");
        }

        if (book.AvailableCopies < 1)
        {
            throw new InvalidOperationException($"No available copies of book '{book.Title}'.");
        }

        var issue = new BookIssue(
            request.BookId,
            request.StudentId,
            request.StaffId,
            request.IssuedToType,
            request.IssueDate,
            request.DueDate,
            request.IssuedBy,
            request.Remarks);

        await _issueRepository.AddAsync(issue, cancellationToken);

        // Decrease available copies
        book.DecreaseAvailableCopies(1);
        await _bookRepository.UpdateAsync(book, cancellationToken);

        _logger.LogInformation(
            "Issued book {BookId} ({Title}) to {IssuedToType}",
            book.Id,
            book.Title,
            request.IssuedToType);

        return issue.Id;
    }
}





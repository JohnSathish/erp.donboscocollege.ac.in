using ERP.Api.Extensions;
using ERP.Application.Library.Commands.CreateBook;
using ERP.Application.Library.Commands.IssueBook;
using ERP.Application.Library.Commands.ReturnBook;
using ERP.Application.Library.Commands.UpdateBook;
using ERP.Application.Library.Queries.GetBook;
using ERP.Application.Library.Queries.ListBooks;
using ERP.Application.Library.Queries.ListBookIssues;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LibraryController> _logger;

    public LibraryController(IMediator mediator, ILogger<LibraryController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("books")]
    public async Task<ActionResult<ListBooksResult>> ListBooks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? category = null,
        [FromQuery] string? author = null,
        [FromQuery] string? status = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListBooksQuery(
            page,
            pageSize,
            category,
            author,
            status != null && Enum.TryParse<ERP.Domain.Library.Entities.BookStatus>(status, true, out var parsedStatus) ? parsedStatus : null,
            searchTerm);

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("books/{id}")]
    public async Task<ActionResult<GetBookResult>> GetBook(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetBookQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("books")]
    public async Task<ActionResult<Guid>> CreateBook(
        [FromBody] CreateBookRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateBookCommand(
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
                this.GetCurrentUserId());

            var bookId = await _mediator.Send(command, cancellationToken);
            return Ok(bookId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating book");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("books/{id}")]
    public async Task<ActionResult> UpdateBook(
        Guid id,
        [FromBody] UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateBookCommand(
                id,
                request.Title,
                request.Author,
                request.Publisher,
                request.PublicationYear,
                request.Category,
                request.Language,
                request.TotalCopies,
                request.Price,
                request.Location,
                request.Description,
                this.GetCurrentUserId());

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating book {BookId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("books/issue")]
    public async Task<ActionResult<Guid>> IssueBook(
        [FromBody] IssueBookRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new IssueBookCommand(
                request.BookId,
                request.StudentId,
                request.StaffId,
                request.IssuedToType,
                request.IssueDate,
                request.DueDate,
                this.GetCurrentUserId(),
                request.Remarks);

            var issueId = await _mediator.Send(command, cancellationToken);
            return Ok(issueId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error issuing book");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("books/return")]
    public async Task<IActionResult> ReturnBook(
        [FromBody] ReturnBookRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new ReturnBookCommand(
                request.IssueId,
                request.ReturnDate,
                request.FineAmount,
                this.GetCurrentUserId(),
                request.Remarks);

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error returning book");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("issues")]
    public async Task<ActionResult<ListBookIssuesResult>> ListBookIssues(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] Guid? bookId = null,
        [FromQuery] Guid? studentId = null,
        [FromQuery] Guid? staffId = null,
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListBookIssuesQuery(
            page,
            pageSize,
            bookId,
            studentId,
            staffId,
            status != null && Enum.TryParse<ERP.Domain.Library.Entities.IssueStatus>(status, true, out var parsedStatus) ? parsedStatus : null);

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}

// Request DTOs
public record CreateBookRequest(
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
    string? CreatedBy = null);

public record IssueBookRequest(
    Guid BookId,
    Guid? StudentId,
    Guid? StaffId,
    string IssuedToType,
    DateTime IssueDate,
    DateTime DueDate,
    string? IssuedBy = null,
    string? Remarks = null);

public record ReturnBookRequest(
    Guid IssueId,
    DateTime ReturnDate,
    decimal? FineAmount = null,
    string? ReturnedBy = null,
    string? Remarks = null);

public record UpdateBookRequest(
    string? Title = null,
    string? Author = null,
    string? Publisher = null,
    int? PublicationYear = null,
    string? Category = null,
    string? Language = null,
    int? TotalCopies = null,
    decimal? Price = null,
    string? Location = null,
    string? Description = null);


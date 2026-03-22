using ERP.Application.Library.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Application.Staff.Interfaces;
using ERP.Domain.Library.Entities;
using MediatR;

namespace ERP.Application.Library.Queries.ListBookIssues;

public sealed class ListBookIssuesQueryHandler : IRequestHandler<ListBookIssuesQuery, ListBookIssuesResult>
{
    private readonly IBookIssueRepository _issueRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IStaffRepository _staffRepository;

    public ListBookIssuesQueryHandler(
        IBookIssueRepository issueRepository,
        IBookRepository bookRepository,
        IStudentRepository studentRepository,
        IStaffRepository staffRepository)
    {
        _issueRepository = issueRepository;
        _bookRepository = bookRepository;
        _studentRepository = studentRepository;
        _staffRepository = staffRepository;
    }

    public async Task<ListBookIssuesResult> Handle(ListBookIssuesQuery request, CancellationToken cancellationToken)
    {
        var (issues, totalCount) = await _issueRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.BookId,
            request.StudentId,
            request.StaffId,
            request.Status,
            cancellationToken);

        var issueDtos = new List<BookIssueDto>();

        foreach (var issue in issues)
        {
            var book = await _bookRepository.GetByIdAsync(issue.BookId, cancellationToken);
            string? studentName = null;
            string? staffName = null;

            if (issue.StudentId.HasValue)
            {
                var student = await _studentRepository.GetByIdAsync(issue.StudentId.Value, cancellationToken);
                studentName = student?.FullName;
            }

            if (issue.StaffId.HasValue)
            {
                var staff = await _staffRepository.GetByIdAsync(issue.StaffId.Value, cancellationToken);
                staffName = staff?.FullName;
            }

            issueDtos.Add(new BookIssueDto(
                issue.Id,
                issue.BookId,
                book?.Title ?? "Unknown",
                book?.Isbn ?? "",
                issue.StudentId,
                studentName,
                issue.StaffId,
                staffName,
                issue.IssuedToType,
                issue.IssueDate,
                issue.DueDate,
                issue.ReturnDate,
                issue.Status.ToString(),
                issue.FineAmount,
                issue.Remarks,
                issue.IssuedBy,
                issue.ReturnedBy,
                issue.CreatedOnUtc));
        }

        return new ListBookIssuesResult(
            issueDtos,
            totalCount,
            request.Page,
            request.PageSize);
    }
}


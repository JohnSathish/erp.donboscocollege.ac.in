using MediatR;

namespace ERP.Application.Library.Commands.IssueBook;

public sealed record IssueBookCommand(
    Guid BookId,
    Guid? StudentId,
    Guid? StaffId,
    string IssuedToType,
    DateTime IssueDate,
    DateTime DueDate,
    string? IssuedBy = null,
    string? Remarks = null) : IRequest<Guid>;





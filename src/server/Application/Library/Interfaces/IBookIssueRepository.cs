using ERP.Domain.Library.Entities;

namespace ERP.Application.Library.Interfaces;

public interface IBookIssueRepository
{
    Task<BookIssue?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<BookIssue>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<BookIssue>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<BookIssue>> GetByStaffIdAsync(Guid staffId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<BookIssue>> GetOverdueIssuesAsync(DateTime currentDate, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<BookIssue> Issues, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? bookId = null,
        Guid? studentId = null,
        Guid? staffId = null,
        IssueStatus? status = null,
        CancellationToken cancellationToken = default);
    Task<BookIssue> AddAsync(BookIssue issue, CancellationToken cancellationToken = default);
    Task UpdateAsync(BookIssue issue, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}





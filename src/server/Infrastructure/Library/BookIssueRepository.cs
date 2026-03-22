using ERP.Application.Library.Interfaces;
using ERP.Domain.Library.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Library;

public class BookIssueRepository : IBookIssueRepository
{
    private readonly ApplicationDbContext _context;

    public BookIssueRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<BookIssue?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.BookIssues
            .FirstOrDefaultAsync(bi => bi.Id == id, cancellationToken);
    }

    public Task<IReadOnlyCollection<BookIssue>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<BookIssue>>(
            _context.BookIssues
                .AsNoTracking()
                .Where(bi => bi.BookId == bookId)
                .OrderByDescending(bi => bi.IssueDate)
                .ToList());
    }

    public Task<IReadOnlyCollection<BookIssue>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<BookIssue>>(
            _context.BookIssues
                .AsNoTracking()
                .Where(bi => bi.StudentId == studentId)
                .OrderByDescending(bi => bi.IssueDate)
                .ToList());
    }

    public Task<IReadOnlyCollection<BookIssue>> GetByStaffIdAsync(Guid staffId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<BookIssue>>(
            _context.BookIssues
                .AsNoTracking()
                .Where(bi => bi.StaffId == staffId)
                .OrderByDescending(bi => bi.IssueDate)
                .ToList());
    }

    public Task<IReadOnlyCollection<BookIssue>> GetOverdueIssuesAsync(DateTime currentDate, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<BookIssue>>(
            _context.BookIssues
                .AsNoTracking()
                .Where(bi => bi.Status == IssueStatus.Issued && bi.DueDate < currentDate)
                .OrderBy(bi => bi.DueDate)
                .ToList());
    }

    public async Task<(IReadOnlyCollection<BookIssue> Issues, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? bookId = null,
        Guid? studentId = null,
        Guid? staffId = null,
        IssueStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.BookIssues.AsNoTracking();

        if (bookId.HasValue)
        {
            query = query.Where(bi => bi.BookId == bookId.Value);
        }

        if (studentId.HasValue)
        {
            query = query.Where(bi => bi.StudentId == studentId.Value);
        }

        if (staffId.HasValue)
        {
            query = query.Where(bi => bi.StaffId == staffId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(bi => bi.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var issues = await query
            .OrderByDescending(bi => bi.IssueDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (issues, totalCount);
    }

    public async Task<BookIssue> AddAsync(BookIssue issue, CancellationToken cancellationToken = default)
    {
        await _context.BookIssues.AddAsync(issue, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return issue;
    }

    public async Task UpdateAsync(BookIssue issue, CancellationToken cancellationToken = default)
    {
        _context.BookIssues.Update(issue);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}





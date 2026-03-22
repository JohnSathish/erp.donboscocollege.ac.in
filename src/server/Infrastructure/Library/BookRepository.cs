using ERP.Application.Library.Interfaces;
using ERP.Domain.Library.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Library;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Books
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public Task<Book?> GetByIsbnAsync(string isbn, CancellationToken cancellationToken = default)
    {
        return _context.Books
            .FirstOrDefaultAsync(b => b.Isbn == isbn, cancellationToken);
    }

    public Task<IReadOnlyCollection<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<Book>>(
            _context.Books
                .AsNoTracking()
                .ToList());
    }

    public async Task<(IReadOnlyCollection<Book> Books, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? category = null,
        string? author = null,
        BookStatus? status = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Books.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(b => b.Category == category);
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(b => b.Author == author);
        }

        if (status.HasValue)
        {
            query = query.Where(b => b.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.Trim();
            query = query.Where(b =>
                b.Isbn.Contains(search) ||
                b.Title.Contains(search) ||
                b.Author.Contains(search) ||
                (b.Publisher != null && b.Publisher.Contains(search)) ||
                (b.Category != null && b.Category.Contains(search)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var books = await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (books, totalCount);
    }

    public async Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await _context.Books.AddAsync(book, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return book;
    }

    public async Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsbnExistsAsync(string isbn, CancellationToken cancellationToken = default)
    {
        return _context.Books
            .AnyAsync(b => b.Isbn == isbn, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}





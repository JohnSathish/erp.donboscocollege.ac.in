using ERP.Domain.Library.Entities;

namespace ERP.Application.Library.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Book?> GetByIsbnAsync(string isbn, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Book>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Book> Books, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? category = null,
        string? author = null,
        BookStatus? status = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);
    Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
    Task<bool> IsbnExistsAsync(string isbn, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}





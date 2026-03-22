using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IProgramRepository
{
    Task<Program?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Program?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Program>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Program> Programs, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        bool? isActive = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    Task<Program> AddAsync(Program program, CancellationToken cancellationToken = default);
    Task UpdateAsync(Program program, CancellationToken cancellationToken = default);
    Task DeleteAsync(Program program, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}










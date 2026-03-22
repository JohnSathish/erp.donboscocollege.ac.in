using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Interfaces;

public interface IScholarshipRepository
{
    Task<Scholarship> AddAsync(Scholarship scholarship, CancellationToken cancellationToken = default);
    Task<Scholarship?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Scholarship>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Scholarship>> GetActiveScholarshipsByStudentIdAsync(Guid studentId, DateTime? asOfDate = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Scholarship>> GetActiveScholarshipsAsync(DateTime? asOfDate = null, CancellationToken cancellationToken = default);
    Task UpdateAsync(Scholarship scholarship, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


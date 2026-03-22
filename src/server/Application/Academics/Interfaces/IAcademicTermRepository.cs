using ERP.Domain.Academics.Entities;

namespace ERP.Application.Academics.Interfaces;

public interface IAcademicTermRepository
{
    Task<AcademicTerm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AcademicTerm?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AcademicTerm>> GetByAcademicYearAsync(string academicYear, CancellationToken cancellationToken = default);
    Task<AcademicTerm?> GetActiveTermAsync(string academicYear, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AcademicTerm>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AcademicTerm> AddAsync(AcademicTerm term, CancellationToken cancellationToken = default);
    Task UpdateAsync(AcademicTerm term, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


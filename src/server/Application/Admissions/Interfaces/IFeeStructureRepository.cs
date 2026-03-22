using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IFeeStructureRepository
{
    Task<FeeStructure?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FeeStructure?> GetByIdWithComponentsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FeeStructure>> GetByProgramIdAsync(Guid programId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FeeStructure>> GetByAcademicYearAsync(string academicYear, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FeeStructure>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<FeeStructure> FeeStructures, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        bool? isActive = null,
        Guid? programId = null,
        string? academicYear = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    Task<FeeStructure> AddAsync(FeeStructure feeStructure, CancellationToken cancellationToken = default);
    Task UpdateAsync(FeeStructure feeStructure, CancellationToken cancellationToken = default);
    Task DeleteAsync(FeeStructure feeStructure, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}










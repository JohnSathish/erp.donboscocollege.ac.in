using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IFeeComponentRepository
{
    Task<FeeComponent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FeeComponent>> GetByFeeStructureIdAsync(Guid feeStructureId, CancellationToken cancellationToken = default);
    Task<FeeComponent> AddAsync(FeeComponent component, CancellationToken cancellationToken = default);
    Task UpdateAsync(FeeComponent component, CancellationToken cancellationToken = default);
    Task DeleteAsync(FeeComponent component, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}










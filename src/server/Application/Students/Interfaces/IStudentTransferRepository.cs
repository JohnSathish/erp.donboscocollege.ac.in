using ERP.Domain.Students.Entities;

namespace ERP.Application.Students.Interfaces;

public interface IStudentTransferRepository
{
    Task<StudentTransfer> AddAsync(StudentTransfer transfer, CancellationToken cancellationToken = default);
    Task<StudentTransfer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<StudentTransfer>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<StudentTransfer>> GetPendingTransfersAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(StudentTransfer transfer, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


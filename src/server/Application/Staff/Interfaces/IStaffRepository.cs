using ERP.Domain.Staff.Entities;

namespace ERP.Application.Staff.Interfaces;

public interface IStaffRepository
{
    Task<StaffMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StaffMember?> GetByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<StaffMember>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<StaffMember> Staff, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? department = null,
        string? employeeType = null,
        StaffStatus? status = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    Task<StaffMember> AddAsync(StaffMember staff, CancellationToken cancellationToken = default);
    Task UpdateAsync(StaffMember staff, CancellationToken cancellationToken = default);
    Task<bool> EmployeeNumberExistsAsync(string employeeNumber, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}





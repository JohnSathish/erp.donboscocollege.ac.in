using ERP.Application.Staff.Interfaces;
using ERP.Domain.Staff.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Staff;

public class StaffRepository : IStaffRepository
{
    private readonly ApplicationDbContext _context;

    public StaffRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<StaffMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.StaffMembers
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task<StaffMember?> GetByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default)
    {
        return _context.StaffMembers
            .FirstOrDefaultAsync(s => s.EmployeeNumber == employeeNumber, cancellationToken);
    }

    public Task<IReadOnlyCollection<StaffMember>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<StaffMember>>(
            _context.StaffMembers
                .AsNoTracking()
                .ToList());
    }

    public async Task<(IReadOnlyCollection<StaffMember> Staff, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? department = null,
        string? employeeType = null,
        StaffStatus? status = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.StaffMembers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(department))
        {
            query = query.Where(s => s.Department == department);
        }

        if (!string.IsNullOrWhiteSpace(employeeType))
        {
            query = query.Where(s => s.EmployeeType == employeeType);
        }

        if (status.HasValue)
        {
            query = query.Where(s => s.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.Trim();
            query = query.Where(s =>
                s.EmployeeNumber.Contains(search) ||
                s.FirstName.Contains(search) ||
                s.LastName.Contains(search) ||
                s.Email.Contains(search) ||
                s.MobileNumber.Contains(search) ||
                (s.Department != null && s.Department.Contains(search)) ||
                (s.Designation != null && s.Designation.Contains(search)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var staff = await query
            .OrderBy(s => s.EmployeeNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (staff, totalCount);
    }

    public async Task<StaffMember> AddAsync(StaffMember staff, CancellationToken cancellationToken = default)
    {
        await _context.StaffMembers.AddAsync(staff, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return staff;
    }

    public async Task UpdateAsync(StaffMember staff, CancellationToken cancellationToken = default)
    {
        _context.StaffMembers.Update(staff);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> EmployeeNumberExistsAsync(string employeeNumber, CancellationToken cancellationToken = default)
    {
        return _context.StaffMembers
            .AnyAsync(s => s.EmployeeNumber == employeeNumber, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}





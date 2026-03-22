using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Students;

public sealed class StudentRepository(ApplicationDbContext context) : IStudentRepository
{
    public async Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Students
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Student?> GetByStudentNumberAsync(string studentNumber, CancellationToken cancellationToken = default)
    {
        return await context.Students
            .FirstOrDefaultAsync(x => x.StudentNumber == studentNumber, cancellationToken);
    }

    public async Task<Student?> GetByApplicantAccountIdAsync(Guid applicantAccountId, CancellationToken cancellationToken = default)
    {
        return await context.Students
            .FirstOrDefaultAsync(x => x.ApplicantAccountId == applicantAccountId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Student>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var query = context.Students.AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(x => isActive.Value ? x.Status == StudentStatus.Active : x.Status != StudentStatus.Active);
        }

        return await query
            .OrderBy(x => x.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyCollection<Student> Students, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        bool? isActive = null,
        Guid? programId = null,
        string? academicYear = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Students.AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(x => isActive.Value ? x.Status == StudentStatus.Active : x.Status != StudentStatus.Active);
        }

        if (programId.HasValue)
        {
            query = query.Where(x => x.ProgramId == programId.Value);
        }

        if (!string.IsNullOrWhiteSpace(academicYear))
        {
            query = query.Where(x => x.AcademicYear == academicYear);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLowerInvariant();
            query = query.Where(x =>
                x.FullName.ToLower().Contains(term) ||
                x.StudentNumber.ToLower().Contains(term) ||
                x.Email.ToLower().Contains(term) ||
                x.MobileNumber.Contains(term) ||
                (x.AdmissionNumber != null && x.AdmissionNumber.ToLower().Contains(term)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var students = await query
            .OrderBy(x => x.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (students, totalCount);
    }

    public async Task<Student> AddAsync(Student student, CancellationToken cancellationToken = default)
    {
        await context.Students.AddAsync(student, cancellationToken);
        return student;
    }

    public async Task UpdateAsync(Student student, CancellationToken cancellationToken = default)
    {
        context.Students.Update(student);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}


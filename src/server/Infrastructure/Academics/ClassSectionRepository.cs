using ERP.Application.Academics.Interfaces;
using ERP.Domain.Academics.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Academics;

public class ClassSectionRepository(ApplicationDbContext context) : IClassSectionRepository
{
    public async Task<ClassSection?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ClassSections
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ClassSection?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ClassSections
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<ClassSection>> GetByCourseIdAsync(Guid courseId, string? academicYear = null, CancellationToken cancellationToken = default)
    {
        var query = context.ClassSections
            .Where(x => x.CourseId == courseId);

        if (!string.IsNullOrWhiteSpace(academicYear))
            query = query.Where(x => x.AcademicYear == academicYear);

        return await query
            .OrderBy(x => x.SectionName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ClassSection>> GetByTeacherIdAsync(Guid teacherId, string? academicYear = null, CancellationToken cancellationToken = default)
    {
        var query = context.ClassSections
            .Where(x => x.TeacherId == teacherId);

        if (!string.IsNullOrWhiteSpace(academicYear))
            query = query.Where(x => x.AcademicYear == academicYear);

        return await query
            .OrderBy(x => x.AcademicYear)
            .ThenBy(x => x.SectionName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ClassSection>> GetByAcademicYearAsync(string academicYear, string? shift = null, CancellationToken cancellationToken = default)
    {
        var query = context.ClassSections
            .Where(x => x.AcademicYear == academicYear);

        if (!string.IsNullOrWhiteSpace(shift))
            query = query.Where(x => x.Shift == shift);

        return await query
            .OrderBy(x => x.SectionName)
            .ToListAsync(cancellationToken);
    }

    public async Task<ClassSection> AddAsync(ClassSection section, CancellationToken cancellationToken = default)
    {
        await context.ClassSections.AddAsync(section, cancellationToken);
        return section;
    }

    public Task UpdateAsync(ClassSection section, CancellationToken cancellationToken = default)
    {
        context.ClassSections.Update(section);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}


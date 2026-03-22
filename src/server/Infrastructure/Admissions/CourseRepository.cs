using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public sealed class CourseRepository(ApplicationDbContext context) : ICourseRepository
{
    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Courses
            .Include(x => x.Program)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Course?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await context.Courses
            .Include(x => x.Program)
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Course>> GetByProgramIdAsync(Guid programId, CancellationToken cancellationToken = default)
    {
        return await context.Courses
            .Include(x => x.Program)
            .Where(x => x.ProgramId == programId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Course>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var query = context.Courses
            .Include(x => x.Program)
            .AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyCollection<Course> Courses, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        bool? isActive = null,
        Guid? programId = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Courses
            .Include(x => x.Program)
            .AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        if (programId.HasValue)
        {
            query = query.Where(x => x.ProgramId == programId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLowerInvariant();
            query = query.Where(x =>
                x.Name.ToLower().Contains(term) ||
                x.Code.ToLower().Contains(term) ||
                (x.Description != null && x.Description.ToLower().Contains(term)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var courses = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (courses, totalCount);
    }

    public async Task<Course> AddAsync(Course course, CancellationToken cancellationToken = default)
    {
        await context.Courses.AddAsync(course, cancellationToken);
        return course;
    }

    public async Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
    {
        context.Courses.Update(course);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Course course, CancellationToken cancellationToken = default)
    {
        context.Courses.Remove(course);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}










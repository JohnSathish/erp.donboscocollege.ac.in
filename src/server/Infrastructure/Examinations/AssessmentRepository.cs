using ERP.Application.Examinations.Interfaces;
using ERP.Domain.Examinations.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Examinations;

public class AssessmentRepository : IAssessmentRepository
{
    private readonly ApplicationDbContext _context;

    public AssessmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Assessment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Assessments
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Assessment?> GetByIdWithComponentsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Assessments
            .Include(a => a.Components.OrderBy(c => c.DisplayOrder))
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Assessment>> GetByCourseAndTermAsync(
        Guid courseId,
        Guid academicTermId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Assessments
            .Where(a => a.CourseId == courseId && a.AcademicTermId == academicTermId)
            .OrderBy(a => a.ScheduledDate)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Assessment>> GetByClassSectionAsync(
        Guid classSectionId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Assessments
            .Where(a => a.ClassSectionId == classSectionId)
            .OrderBy(a => a.ScheduledDate)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Assessment>> GetByAcademicTermAsync(
        Guid academicTermId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Assessments
            .Where(a => a.AcademicTermId == academicTermId)
            .OrderBy(a => a.ScheduledDate)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Assessment> AddAsync(Assessment assessment, CancellationToken cancellationToken = default)
    {
        await _context.Assessments.AddAsync(assessment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return assessment;
    }

    public async Task UpdateAsync(Assessment assessment, CancellationToken cancellationToken = default)
    {
        _context.Assessments.Update(assessment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Assessments
            .AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<bool> CodeExistsAsync(
        string code,
        Guid courseId,
        Guid academicTermId,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Assessments
            .Where(a => a.Code == code.ToUpperInvariant() &&
                       a.CourseId == courseId &&
                       a.AcademicTermId == academicTermId);

        if (excludeId.HasValue)
        {
            query = query.Where(a => a.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}






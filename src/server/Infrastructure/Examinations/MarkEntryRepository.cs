using ERP.Application.Examinations.Interfaces;
using ERP.Domain.Examinations.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Examinations;

public class MarkEntryRepository : IMarkEntryRepository
{
    private readonly ApplicationDbContext _context;

    public MarkEntryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MarkEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.MarkEntries
            .Include(me => me.AssessmentComponent)
            .FirstOrDefaultAsync(me => me.Id == id, cancellationToken);
    }

    public async Task<MarkEntry?> GetByComponentAndStudentAsync(
        Guid assessmentComponentId,
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        return await _context.MarkEntries
            .Include(me => me.AssessmentComponent)
            .FirstOrDefaultAsync(me => me.AssessmentComponentId == assessmentComponentId &&
                                      me.StudentId == studentId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<MarkEntry>> GetByAssessmentComponentIdAsync(
        Guid assessmentComponentId,
        CancellationToken cancellationToken = default)
    {
        return await _context.MarkEntries
            .Where(me => me.AssessmentComponentId == assessmentComponentId)
            .OrderBy(me => me.StudentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<MarkEntry>> GetByStudentIdAsync(
        Guid studentId,
        Guid? academicTermId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.MarkEntries
            .Include(me => me.AssessmentComponent)
                .ThenInclude(ac => ac.Assessment)
            .Where(me => me.StudentId == studentId);

        if (academicTermId.HasValue)
        {
            query = query.Where(me => me.AssessmentComponent.Assessment.AcademicTermId == academicTermId.Value);
        }

        return await query
            .OrderBy(me => me.AssessmentComponent.Assessment.ScheduledDate)
            .ThenBy(me => me.AssessmentComponent.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<MarkEntry>> GetByAssessmentIdAsync(
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        return await _context.MarkEntries
            .Include(me => me.AssessmentComponent)
            .Where(me => me.AssessmentComponent.AssessmentId == assessmentId)
            .OrderBy(me => me.StudentId)
            .ThenBy(me => me.AssessmentComponent.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<MarkEntry> AddAsync(MarkEntry markEntry, CancellationToken cancellationToken = default)
    {
        await _context.MarkEntries.AddAsync(markEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return markEntry;
    }

    public async Task UpdateAsync(MarkEntry markEntry, CancellationToken cancellationToken = default)
    {
        _context.MarkEntries.Update(markEntry);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<MarkEntry> markEntries, CancellationToken cancellationToken = default)
    {
        await _context.MarkEntries.AddRangeAsync(markEntries, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<MarkEntry> markEntries, CancellationToken cancellationToken = default)
    {
        _context.MarkEntries.UpdateRange(markEntries);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid assessmentComponentId,
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        return await _context.MarkEntries
            .AnyAsync(me => me.AssessmentComponentId == assessmentComponentId &&
                          me.StudentId == studentId, cancellationToken);
    }
}


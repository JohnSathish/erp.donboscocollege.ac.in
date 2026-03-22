using ERP.Application.Examinations.Interfaces;
using ERP.Domain.Examinations.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Examinations;

public class AssessmentComponentRepository : IAssessmentComponentRepository
{
    private readonly ApplicationDbContext _context;

    public AssessmentComponentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AssessmentComponent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AssessmentComponents
            .FirstOrDefaultAsync(ac => ac.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<AssessmentComponent>> GetByAssessmentIdAsync(
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        return await _context.AssessmentComponents
            .Where(ac => ac.AssessmentId == assessmentId)
            .OrderBy(ac => ac.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<AssessmentComponent> AddAsync(
        AssessmentComponent component,
        CancellationToken cancellationToken = default)
    {
        await _context.AssessmentComponents.AddAsync(component, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return component;
    }

    public async Task UpdateAsync(AssessmentComponent component, CancellationToken cancellationToken = default)
    {
        _context.AssessmentComponents.Update(component);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(AssessmentComponent component, CancellationToken cancellationToken = default)
    {
        _context.AssessmentComponents.Remove(component);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByAssessmentIdAsync(Guid assessmentId, CancellationToken cancellationToken = default)
    {
        var components = await _context.AssessmentComponents
            .Where(ac => ac.AssessmentId == assessmentId)
            .ToListAsync(cancellationToken);

        _context.AssessmentComponents.RemoveRange(components);
        await _context.SaveChangesAsync(cancellationToken);
    }
}






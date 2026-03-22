using ERP.Application.Academics.Interfaces;
using ERP.Domain.Academics.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Academics;

public class AcademicTermRepository(ApplicationDbContext context) : IAcademicTermRepository
{
    public async Task<AcademicTerm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.AcademicTerms
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<AcademicTerm?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.AcademicTerms
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<AcademicTerm>> GetByAcademicYearAsync(string academicYear, CancellationToken cancellationToken = default)
    {
        return await context.AcademicTerms
            .Where(x => x.AcademicYear == academicYear)
            .OrderBy(x => x.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<AcademicTerm?> GetActiveTermAsync(string academicYear, CancellationToken cancellationToken = default)
    {
        return await context.AcademicTerms
            .Where(x => x.AcademicYear == academicYear && x.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AcademicTerm>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.AcademicTerms
            .OrderByDescending(x => x.AcademicYear)
            .ThenByDescending(x => x.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<AcademicTerm> AddAsync(AcademicTerm term, CancellationToken cancellationToken = default)
    {
        await context.AcademicTerms.AddAsync(term, cancellationToken);
        return term;
    }

    public Task UpdateAsync(AcademicTerm term, CancellationToken cancellationToken = default)
    {
        context.AcademicTerms.Update(term);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}


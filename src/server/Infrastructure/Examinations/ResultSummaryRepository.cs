using ERP.Application.Examinations.Interfaces;
using ERP.Domain.Examinations.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Examinations;

public class ResultSummaryRepository : IResultSummaryRepository
{
    private readonly ApplicationDbContext _context;

    public ResultSummaryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResultSummary?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ResultSummaries
            .FirstOrDefaultAsync(rs => rs.Id == id, cancellationToken);
    }

    public async Task<ResultSummary?> GetByIdWithCourseResultsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ResultSummaries
            .Include(rs => rs.CourseResults.OrderBy(cr => cr.CourseId))
            .FirstOrDefaultAsync(rs => rs.Id == id, cancellationToken);
    }

    public async Task<ResultSummary?> GetByStudentAndTermAsync(
        Guid studentId,
        Guid academicTermId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ResultSummaries
            .Include(rs => rs.CourseResults)
            .FirstOrDefaultAsync(rs => rs.StudentId == studentId &&
                                     rs.AcademicTermId == academicTermId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<ResultSummary>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ResultSummaries
            .Where(rs => rs.StudentId == studentId)
            .OrderByDescending(rs => rs.AcademicTermId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ResultSummary>> GetByAcademicTermIdAsync(
        Guid academicTermId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ResultSummaries
            .Where(rs => rs.AcademicTermId == academicTermId)
            .OrderBy(rs => rs.StudentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ResultSummary> AddAsync(ResultSummary resultSummary, CancellationToken cancellationToken = default)
    {
        await _context.ResultSummaries.AddAsync(resultSummary, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return resultSummary;
    }

    public async Task UpdateAsync(ResultSummary resultSummary, CancellationToken cancellationToken = default)
    {
        _context.ResultSummaries.Update(resultSummary);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid studentId,
        Guid academicTermId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ResultSummaries
            .AnyAsync(rs => rs.StudentId == studentId &&
                          rs.AcademicTermId == academicTermId, cancellationToken);
    }
}






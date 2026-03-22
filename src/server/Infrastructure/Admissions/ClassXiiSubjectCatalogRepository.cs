using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public sealed class ClassXiiSubjectCatalogRepository : IClassXiiSubjectCatalogRepository
{
    private readonly ApplicationDbContext _context;

    public ClassXiiSubjectCatalogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ClassXiiSubjectOptionDto>> ListActiveByBoardAndStreamAsync(
        string boardCode,
        string streamCode,
        CancellationToken cancellationToken = default)
    {
        var b = boardCode.Trim().ToUpperInvariant();
        var s = streamCode.Trim().ToUpperInvariant();

        return await _context.ClassXiiSubjectMasters
            .AsNoTracking()
            .Where(x => x.IsActive && x.BoardCode == b && x.StreamCode == s)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.SubjectName)
            .Select(x => new ClassXiiSubjectOptionDto(x.Id, x.SubjectName, x.SortOrder))
            .ToListAsync(cancellationToken);
    }

    public async Task ReplaceEntireCatalogAsync(
        IReadOnlyList<ClassXiiSubjectCatalogRow> rows,
        CancellationToken cancellationToken = default)
    {
        await _context.ClassXiiSubjectMasters.ExecuteDeleteAsync(cancellationToken);

        if (rows.Count == 0)
        {
            return;
        }

        var entities = rows.Select(r => new ClassXiiSubjectMaster
        {
            Id = Guid.NewGuid(),
            BoardCode = r.BoardCode.Trim().ToUpperInvariant(),
            StreamCode = r.StreamCode.Trim().ToUpperInvariant(),
            SubjectName = r.SubjectName.Trim(),
            SortOrder = r.SortOrder,
            IsActive = true
        }).ToList();

        _context.ClassXiiSubjectMasters.AddRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

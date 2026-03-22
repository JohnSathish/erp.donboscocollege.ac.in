using ERP.Application.Admissions.ViewModels;

namespace ERP.Application.Admissions.Interfaces;

public interface IClassXiiSubjectCatalogRepository
{
    Task<IReadOnlyList<ClassXiiSubjectOptionDto>> ListActiveByBoardAndStreamAsync(
        string boardCode,
        string streamCode,
        CancellationToken cancellationToken = default);

    Task ReplaceEntireCatalogAsync(
        IReadOnlyList<ClassXiiSubjectCatalogRow> rows,
        CancellationToken cancellationToken = default);
}

public sealed record ClassXiiSubjectCatalogRow(string BoardCode, string StreamCode, string SubjectName, int SortOrder);

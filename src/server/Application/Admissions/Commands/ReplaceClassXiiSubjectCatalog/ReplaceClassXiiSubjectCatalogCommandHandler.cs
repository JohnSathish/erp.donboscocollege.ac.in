using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.ReplaceClassXiiSubjectCatalog;

public sealed class ReplaceClassXiiSubjectCatalogCommandHandler
    : IRequestHandler<ReplaceClassXiiSubjectCatalogCommand, ReplaceClassXiiSubjectCatalogResult>
{
    private static readonly HashSet<string> AllowedBoards = new(StringComparer.OrdinalIgnoreCase)
    {
        "MBOSE", "CBSE", "ISC"
    };

    private static readonly HashSet<string> AllowedStreams = new(StringComparer.OrdinalIgnoreCase)
    {
        "ARTS", "SCIENCE", "COMMERCE"
    };

    private readonly IClassXiiSubjectCatalogRepository _catalog;

    public ReplaceClassXiiSubjectCatalogCommandHandler(IClassXiiSubjectCatalogRepository catalog)
    {
        _catalog = catalog;
    }

    public async Task<ReplaceClassXiiSubjectCatalogResult> Handle(
        ReplaceClassXiiSubjectCatalogCommand request,
        CancellationToken cancellationToken)
    {
        var normalized = new List<ClassXiiSubjectCatalogRow>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in request.Rows)
        {
            var board = (row.BoardCode ?? string.Empty).Trim();
            var stream = (row.StreamCode ?? string.Empty).Trim();
            var name = (row.SubjectName ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(board) || string.IsNullOrEmpty(stream) || string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Each row must have non-empty boardCode, streamCode, and subjectName.");
            }

            if (!AllowedBoards.Contains(board))
            {
                throw new ArgumentException($"Invalid board code: {board}");
            }

            if (!AllowedStreams.Contains(stream))
            {
                throw new ArgumentException($"Invalid stream code: {stream}");
            }

            var key = $"{board}|{stream}|{name}";
            if (!seen.Add(key))
            {
                throw new ArgumentException($"Duplicate subject in import: {board} / {stream} / {name}");
            }

            normalized.Add(new ClassXiiSubjectCatalogRow(
                board.ToUpperInvariant(),
                stream.ToUpperInvariant(),
                name,
                row.SortOrder));
        }

        await _catalog.ReplaceEntireCatalogAsync(normalized, cancellationToken);
        return new ReplaceClassXiiSubjectCatalogResult(normalized.Count);
    }
}

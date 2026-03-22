using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListClassXiiSubjects;

public sealed class ListClassXiiSubjectsQueryHandler
    : IRequestHandler<ListClassXiiSubjectsQuery, IReadOnlyList<ClassXiiSubjectOptionDto>>
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

    public ListClassXiiSubjectsQueryHandler(IClassXiiSubjectCatalogRepository catalog)
    {
        _catalog = catalog;
    }

    public async Task<IReadOnlyList<ClassXiiSubjectOptionDto>> Handle(
        ListClassXiiSubjectsQuery request,
        CancellationToken cancellationToken)
    {
        var board = (request.Board ?? string.Empty).Trim();
        var stream = (request.Stream ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(board) || string.IsNullOrEmpty(stream))
        {
            throw new ArgumentException("Board and stream query parameters are required.");
        }

        if (!AllowedBoards.Contains(board))
        {
            throw new ArgumentException("Invalid board. Use MBOSE, CBSE, or ISC.");
        }

        if (!AllowedStreams.Contains(stream))
        {
            throw new ArgumentException("Invalid stream. Use ARTS, SCIENCE, or COMMERCE.");
        }

        return await _catalog.ListActiveByBoardAndStreamAsync(board, stream, cancellationToken);
    }
}

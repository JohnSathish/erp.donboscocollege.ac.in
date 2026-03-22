using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.ReplaceClassXiiSubjectCatalog;

public sealed record ReplaceClassXiiSubjectCatalogCommand(
    IReadOnlyList<ClassXiiSubjectCatalogRow> Rows) : IRequest<ReplaceClassXiiSubjectCatalogResult>;

public sealed record ReplaceClassXiiSubjectCatalogResult(int ImportedRowCount);

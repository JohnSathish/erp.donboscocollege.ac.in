using MediatR;

namespace ERP.Application.Admissions.Queries.DownloadDocument;

public sealed record DownloadDocumentQuery(
    Guid AccountId,
    string DocumentType) : IRequest<DocumentDownloadDto?>;

public sealed record DocumentDownloadDto(
    byte[] Content,
    string FileName,
    string ContentType);












using MediatR;

namespace ERP.Application.Admissions.Commands.UploadDocument;

public sealed record UploadDocumentCommand(
    Guid AccountId,
    string DocumentType,
    string FileName,
    string ContentType,
    byte[] FileData) : IRequest<UploadDocumentResult>;

public sealed record UploadDocumentResult(
    string DocumentType,
    string FileName,
    long FileSizeBytes,
    DateTime UploadedOnUtc);


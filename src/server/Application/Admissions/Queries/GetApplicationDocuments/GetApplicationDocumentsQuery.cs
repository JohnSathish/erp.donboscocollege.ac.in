using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetApplicationDocuments;

public sealed record GetApplicationDocumentsQuery(Guid AccountId) : IRequest<ApplicationDocumentsDto?>;

public sealed record ApplicationDocumentsDto(
    Guid AccountId,
    string ApplicationNumber,
    string FullName,
    IReadOnlyCollection<DocumentDto> Documents);

public sealed record DocumentDto(
    string DocumentType,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    bool IsUploaded,
    DocumentVerificationStatusDto? VerificationStatus = null);

public sealed record DocumentVerificationStatusDto(
    bool IsVerified,
    DateTime VerifiedOnUtc,
    string? VerifiedBy,
    string? Remarks);


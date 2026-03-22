using MediatR;

namespace ERP.Application.Admissions.Commands.VerifyDocument;

public sealed record VerifyDocumentCommand(
    Guid AccountId,
    string DocumentType,
    bool IsVerified,
    string? Remarks = null,
    string? VerifiedBy = null) : IRequest<bool>;












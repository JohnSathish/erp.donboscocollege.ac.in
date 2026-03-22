using MediatR;

namespace ERP.Application.Admissions.Commands.RejectAdmissionOffer;

public sealed record RejectAdmissionOfferCommand(
    Guid OfferId,
    string? Reason = null) : IRequest<RejectAdmissionOfferResult>;

public sealed record RejectAdmissionOfferResult(
    Guid OfferId,
    string ApplicationNumber,
    bool Success,
    string Message);


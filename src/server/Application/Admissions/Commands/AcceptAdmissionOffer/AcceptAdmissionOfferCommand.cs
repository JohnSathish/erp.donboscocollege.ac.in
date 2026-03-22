using MediatR;

namespace ERP.Application.Admissions.Commands.AcceptAdmissionOffer;

public sealed record AcceptAdmissionOfferCommand(Guid OfferId) : IRequest<AcceptAdmissionOfferResult>;

public sealed record AcceptAdmissionOfferResult(
    Guid OfferId,
    string ApplicationNumber,
    bool Success,
    string Message);


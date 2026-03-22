using MediatR;

namespace ERP.Application.Admissions.Commands.CreateAdmissionOffer;

public sealed record CreateAdmissionOfferCommand(
    Guid AccountId,
    DateTime ExpiryDate,
    string? Remarks = null,
    string? CreatedBy = null) : IRequest<CreateAdmissionOfferResult>;

public sealed record CreateAdmissionOfferResult(
    Guid OfferId,
    string ApplicationNumber,
    string FullName,
    int MeritRank,
    DateTime OfferDate,
    DateTime ExpiryDate);


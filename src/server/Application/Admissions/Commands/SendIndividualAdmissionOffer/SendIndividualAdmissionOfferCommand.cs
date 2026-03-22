using MediatR;

namespace ERP.Application.Admissions.Commands.SendIndividualAdmissionOffer;

public sealed record SendIndividualAdmissionOfferCommand(
    string ApplicationNumber,
    decimal AdmissionFeeAmount,
    int ExpiryDays = 2,
    string? Remarks = null,
    string? CreatedBy = null) : IRequest<SendIndividualAdmissionOfferResult>;

public sealed record SendIndividualAdmissionOfferResult(
    Guid OfferId,
    string ApplicationNumber,
    string FullName,
    string Email,
    DateTime OfferDate,
    DateTime ExpiryDate,
    decimal AdmissionFeeAmount);





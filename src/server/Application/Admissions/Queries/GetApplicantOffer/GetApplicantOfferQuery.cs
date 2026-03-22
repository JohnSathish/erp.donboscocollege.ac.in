using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetApplicantOffer;

public sealed record GetApplicantOfferQuery(Guid AccountId) : IRequest<AdmissionOfferDto?>;

public sealed record AdmissionOfferDto(
    Guid Id,
    string ApplicationNumber,
    string FullName,
    int MeritRank,
    string Shift,
    string MajorSubject,
    string Status,
    DateTime OfferDate,
    DateTime ExpiryDate,
    DateTime? AcceptedOnUtc,
    DateTime? RejectedOnUtc,
    string? Remarks);


using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetApplicantOffer;

public sealed class GetApplicantOfferQueryHandler : IRequestHandler<GetApplicantOfferQuery, AdmissionOfferDto?>
{
    private readonly IAdmissionsRepository _admissionsRepository;

    public GetApplicantOfferQueryHandler(IAdmissionsRepository admissionsRepository)
    {
        _admissionsRepository = admissionsRepository;
    }

    public async Task<AdmissionOfferDto?> Handle(GetApplicantOfferQuery request, CancellationToken cancellationToken)
    {
        var offer = await _admissionsRepository.GetOfferByAccountIdAsync(request.AccountId, cancellationToken);
        if (offer == null)
        {
            return null;
        }

        // Mark expired if needed
        offer.MarkExpired();
        if (offer.Status == Domain.Admissions.Entities.OfferStatus.Expired)
        {
            await _admissionsRepository.UpdateAdmissionOfferAsync(offer, cancellationToken);
        }

        return new AdmissionOfferDto(
            offer.Id,
            offer.ApplicationNumber,
            offer.FullName,
            offer.MeritRank,
            offer.Shift,
            offer.MajorSubject,
            offer.Status.ToString(),
            offer.OfferDate,
            offer.ExpiryDate,
            offer.AcceptedOnUtc,
            offer.RejectedOnUtc,
            offer.Remarks);
    }
}


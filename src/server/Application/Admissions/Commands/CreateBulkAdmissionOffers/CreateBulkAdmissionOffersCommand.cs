using MediatR;

namespace ERP.Application.Admissions.Commands.CreateBulkAdmissionOffers;

public sealed record CreateBulkAdmissionOffersCommand(
    string? Shift = null,
    string? MajorSubject = null,
    int? TopNRanks = null,
    DateTime? ExpiryDate = null,
    string? CreatedBy = null) : IRequest<CreateBulkAdmissionOffersResult>;

public sealed record CreateBulkAdmissionOffersResult(
    int TotalOffersCreated,
    List<Guid> CreatedOfferIds,
    List<string> Errors);


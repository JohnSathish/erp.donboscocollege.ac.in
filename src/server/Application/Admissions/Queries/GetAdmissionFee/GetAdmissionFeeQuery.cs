using MediatR;

namespace ERP.Application.Admissions.Queries.GetAdmissionFee;

public sealed record GetAdmissionFeeQuery(Guid AccountId) : IRequest<AdmissionFeeDto>;

public sealed record AdmissionFeeDto(
    decimal RequiredAmount,
    decimal? PaidAmount,
    bool IsPaymentValid,
    string? MajorSubject,
    string Message);



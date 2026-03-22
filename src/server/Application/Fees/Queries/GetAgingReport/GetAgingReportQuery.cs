using MediatR;
using ERP.Application.Fees.Interfaces;

namespace ERP.Application.Fees.Queries.GetAgingReport;

public sealed record GetAgingReportQuery(
    DateTime? AsOfDate = null,
    Guid? StudentId = null) : IRequest<AgingReportDto>;


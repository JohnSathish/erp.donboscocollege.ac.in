using MediatR;
using ERP.Application.Fees.Interfaces;

namespace ERP.Application.Fees.Queries.GetStudentFeeLedger;

public sealed record GetStudentFeeLedgerQuery(
    Guid StudentId,
    string? AcademicYear = null) : IRequest<FeeLedgerSummaryDto?>;


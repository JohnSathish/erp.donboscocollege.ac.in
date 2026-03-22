using ERP.Application.Fees.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Queries.GetAgingReport;

public sealed class GetAgingReportQueryHandler : IRequestHandler<GetAgingReportQuery, AgingReportDto>
{
    private readonly IFeeLedgerService _feeLedgerService;
    private readonly ILogger<GetAgingReportQueryHandler> _logger;

    public GetAgingReportQueryHandler(
        IFeeLedgerService feeLedgerService,
        ILogger<GetAgingReportQueryHandler> logger)
    {
        _feeLedgerService = feeLedgerService;
        _logger = logger;
    }

    public async Task<AgingReportDto> Handle(GetAgingReportQuery request, CancellationToken cancellationToken)
    {
        return await _feeLedgerService.GetAgingReportAsync(
            request.AsOfDate,
            request.StudentId,
            cancellationToken);
    }
}


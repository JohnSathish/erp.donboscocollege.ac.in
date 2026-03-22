using ERP.Application.Fees.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Queries.GetStudentFeeLedger;

public sealed class GetStudentFeeLedgerQueryHandler : IRequestHandler<GetStudentFeeLedgerQuery, FeeLedgerSummaryDto?>
{
    private readonly IFeeLedgerService _feeLedgerService;
    private readonly ILogger<GetStudentFeeLedgerQueryHandler> _logger;

    public GetStudentFeeLedgerQueryHandler(
        IFeeLedgerService feeLedgerService,
        ILogger<GetStudentFeeLedgerQueryHandler> logger)
    {
        _feeLedgerService = feeLedgerService;
        _logger = logger;
    }

    public async Task<FeeLedgerSummaryDto?> Handle(GetStudentFeeLedgerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _feeLedgerService.GetStudentFeeLedgerAsync(
                request.StudentId,
                request.AcademicYear,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving fee ledger for student {StudentId}.", request.StudentId);
            return null;
        }
    }
}


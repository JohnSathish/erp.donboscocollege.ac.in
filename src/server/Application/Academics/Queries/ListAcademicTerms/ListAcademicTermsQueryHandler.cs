using ERP.Application.Academics.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Academics.Queries.ListAcademicTerms;

public sealed class ListAcademicTermsQueryHandler : IRequestHandler<ListAcademicTermsQuery, IReadOnlyCollection<AcademicTermDto>>
{
    private readonly IAcademicTermRepository _termRepository;
    private readonly ILogger<ListAcademicTermsQueryHandler> _logger;

    public ListAcademicTermsQueryHandler(
        IAcademicTermRepository termRepository,
        ILogger<ListAcademicTermsQueryHandler> logger)
    {
        _termRepository = termRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<AcademicTermDto>> Handle(ListAcademicTermsQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Domain.Academics.Entities.AcademicTerm> terms;

        if (!string.IsNullOrWhiteSpace(request.AcademicYear))
        {
            terms = await _termRepository.GetByAcademicYearAsync(request.AcademicYear, cancellationToken);
        }
        else
        {
            terms = await _termRepository.GetAllAsync(cancellationToken);
        }

        if (request.IsActive.HasValue)
        {
            terms = terms.Where(t => t.IsActive == request.IsActive.Value).ToList();
        }

        return terms.Select(t => new AcademicTermDto(
            t.Id,
            t.TermName,
            t.AcademicYear,
            t.StartDate,
            t.EndDate,
            t.IsActive,
            t.Remarks,
            t.CreatedOnUtc,
            t.CreatedBy)).ToList();
    }
}


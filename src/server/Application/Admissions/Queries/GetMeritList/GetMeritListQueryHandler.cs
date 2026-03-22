using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetMeritList;

public sealed class GetMeritListQueryHandler : IRequestHandler<GetMeritListQuery, MeritListResponse>
{
    private readonly IAdmissionsRepository _admissionsRepository;
    private readonly IAdmissionWorkflowSettingsService _workflowSettings;

    public GetMeritListQueryHandler(
        IAdmissionsRepository admissionsRepository,
        IAdmissionWorkflowSettingsService workflowSettings)
    {
        _admissionsRepository = admissionsRepository;
        _workflowSettings = workflowSettings;
    }

    public async Task<MeritListResponse> Handle(GetMeritListQuery request, CancellationToken cancellationToken)
    {
        var cutoff = await _workflowSettings.GetMeritClassXiiCutoffPercentageAsync(cancellationToken);
        var (meritScores, totalCount) = await _admissionsRepository.GetMeritScoresAsync(
            request.Shift,
            request.MajorSubject,
            request.Page,
            request.PageSize,
            cutoff,
            cancellationToken);

        var dtos = meritScores.Select(ms => new MeritScoreDto(
            ms.Id,
            ms.AccountId,
            ms.ApplicationNumber,
            ms.FullName,
            ms.ClassXIIPercentage,
            ms.CuetScore,
            ms.EntranceExamScore,
            ms.TotalScore,
            ms.Rank,
            ms.Shift,
            ms.MajorSubject,
            ms.CalculatedOnUtc)).ToList();

        return new MeritListResponse(
            dtos,
            totalCount,
            request.Page,
            request.PageSize);
    }
}


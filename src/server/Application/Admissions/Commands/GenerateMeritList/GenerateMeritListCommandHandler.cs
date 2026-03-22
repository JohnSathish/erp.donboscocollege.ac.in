using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.GenerateMeritList;

public sealed class GenerateMeritListCommandHandler : IRequestHandler<GenerateMeritListCommand, GenerateMeritListResult>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly IAdmissionsRepository _admissionsRepository;
    private readonly IAdmissionWorkflowSettingsService _workflowSettings;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    // Scoring weights (configurable in future)
    private const decimal ClassXIIWeight = 0.5m; // 50%
    private const decimal CuetWeight = 0.3m;      // 30%
    private const decimal EntranceExamWeight = 0.2m; // 20%

    public GenerateMeritListCommandHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository,
        IAdmissionsRepository admissionsRepository,
        IAdmissionWorkflowSettingsService workflowSettings)
    {
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
        _admissionsRepository = admissionsRepository;
        _workflowSettings = workflowSettings;
    }

    public async Task<GenerateMeritListResult> Handle(GenerateMeritListCommand request, CancellationToken cancellationToken)
    {
        // Get all eligible applicants (submitted application + payment completed)
        var eligibleAccounts = await _accountRepository.GetEligibleForMeritListAsync(
            request.Shift,
            request.MajorSubject,
            cancellationToken);

        var eligibleList = eligibleAccounts.ToList();
        var eligibleAccountIds = eligibleList.Select(a => a.Id).ToHashSet();
        var meritCutoff = await _workflowSettings.GetMeritClassXiiCutoffPercentageAsync(cancellationToken);

        var meritScores = new List<MeritScore>();
        var processedCount = 0;

        foreach (var account in eligibleList)
        {
            processedCount++;

            // Get application draft
            var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(account.Id, cancellationToken);
            if (draftEntity == null)
            {
                continue;
            }

            var draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _serializerOptions)
                       ?? ApplicantApplicationDraftDto.Empty;

            // Calculate scores (prefer synced account percentage when present)
            var parsedDraftPercentage = ParsePercentage(draft.Academics?.BoardExamination?.Percentage);
            var classXIIPercentage = account.ClassXIIPercentage ?? parsedDraftPercentage;

            if (classXIIPercentage < meritCutoff)
            {
                continue;
            }
            var cuetScore = ParseDecimal(draft.Academics?.Cuet?.Marks);
            var entranceExamScore = await GetEntranceExamScoreAsync(account.Id, cancellationToken);

            // Calculate total score
            var totalScore = CalculateTotalScore(classXIIPercentage, cuetScore, entranceExamScore);

            // Get shift and major subject
            var shift = account.Shift ?? draft.Courses?.Shift ?? string.Empty;
            var majorSubject = draft.Courses?.MajorSubject ?? string.Empty;

            // Create merit score (rank will be assigned after sorting)
            var meritScore = new MeritScore(
                account.Id,
                account.UniqueId,
                account.FullName,
                classXIIPercentage,
                cuetScore,
                entranceExamScore,
                totalScore,
                0, // Rank will be set after sorting
                shift,
                majorSubject,
                request.CreatedBy);

            meritScores.Add(meritScore);
        }

        // Group by shift and major subject, then rank within each group
        var groupedScores = meritScores
            .GroupBy(ms => new { ms.Shift, ms.MajorSubject })
            .ToList();

        var allRankedScores = new List<MeritScore>();

        foreach (var group in groupedScores)
        {
            var rankedGroup = group
                .OrderByDescending(ms => ms.TotalScore)
                .ThenByDescending(ms => ms.ClassXIIPercentage)
                .ThenByDescending(ms => ms.CuetScore ?? 0)
                .Select((ms, index) =>
                {
                    ms.UpdateRank(index + 1);
                    return ms;
                })
                .ToList();

            allRankedScores.AddRange(rankedGroup);
        }

        var includedAccountIds = allRankedScores.Select(ms => ms.AccountId).ToHashSet();
        var removeMeritForAccountIds = eligibleAccountIds.Except(includedAccountIds).ToList();

        await _admissionsRepository.UpsertMeritScoresAsync(allRankedScores, removeMeritForAccountIds, cancellationToken);

        return new GenerateMeritListResult(
            processedCount,
            allRankedScores.Count,
            DateTime.UtcNow);
    }

    private decimal CalculateTotalScore(
        decimal classXIIPercentage,
        decimal? cuetScore,
        decimal? entranceExamScore)
    {
        var score = classXIIPercentage * ClassXIIWeight;

        if (cuetScore.HasValue)
        {
            // Normalize CUET score to percentage (assuming max 800)
            var cuetPercentage = (cuetScore.Value / 800m) * 100m;
            score += cuetPercentage * CuetWeight;
        }

        if (entranceExamScore.HasValue)
        {
            // Normalize entrance exam score to percentage (assuming max 100)
            var examPercentage = entranceExamScore.Value;
            score += examPercentage * EntranceExamWeight;
        }

        return Math.Round(score, 2);
    }

    private decimal ParsePercentage(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0;
        }

        // Remove % sign if present
        var cleaned = value.Trim().Replace("%", "").Trim();
        if (decimal.TryParse(cleaned, out var result))
        {
            return Math.Clamp(result, 0, 100);
        }

        return 0;
    }

    private decimal? ParseDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (decimal.TryParse(value.Trim(), out var result))
        {
            return result;
        }

        return null;
    }

    private async Task<decimal?> GetEntranceExamScoreAsync(Guid accountId, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to get entrance exam score from ExamRegistration
        // For now, return null
        return null;
    }
}


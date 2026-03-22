using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetPublishedSelectionList;

public sealed class GetPublishedSelectionListQueryHandler
    : IRequestHandler<GetPublishedSelectionListQuery, IReadOnlyList<PublishedSelectionListEntryDto>>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public GetPublishedSelectionListQueryHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository)
    {
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
    }

    public async Task<IReadOnlyList<PublishedSelectionListEntryDto>> Handle(
        GetPublishedSelectionListQuery request,
        CancellationToken cancellationToken)
    {
        var accounts = (await _accountRepository.GetPublishedSelectionListAsync(request.Round, cancellationToken))
            .ToList();

        if (accounts.Count == 0)
        {
            return Array.Empty<PublishedSelectionListEntryDto>();
        }

        var ids = accounts.Select(a => a.Id).ToList();
        var drafts = await _applicationRepository.GetDraftsByAccountIdsAsync(ids, cancellationToken);

        var majorByAccount = new Dictionary<Guid, string?>();
        foreach (var draft in drafts)
        {
            try
            {
                var dto = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draft.Data, _serializerOptions);
                var major = string.IsNullOrWhiteSpace(dto?.Courses?.MajorSubject)
                    ? null
                    : dto!.Courses.MajorSubject.Trim();
                majorByAccount[draft.AccountId] = major;
            }
            catch (JsonException)
            {
                // ignore
            }
        }

        return accounts
            .Select(a =>
            {
                _ = majorByAccount.TryGetValue(a.Id, out var draftMajor);
                var major = draftMajor ?? a.OfflineIssuedMajorSubject;
                return new PublishedSelectionListEntryDto(
                    a.UniqueId,
                    a.FullName,
                    major,
                    a.SelectionListRound!.Value,
                    a.SelectionListPublishedAtUtc!.Value);
            })
            .ToList();
    }
}

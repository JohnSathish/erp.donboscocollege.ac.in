using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListAdmittedStudents;

public sealed class ListAdmittedStudentsQueryHandler : IRequestHandler<ListAdmittedStudentsQuery, AdmittedStudentsListResponse>
{
    private readonly IApplicantAccountRepository _accounts;
    private readonly IApplicantApplicationRepository _drafts;
    private readonly JsonSerializerOptions _json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public ListAdmittedStudentsQueryHandler(
        IApplicantAccountRepository accounts,
        IApplicantApplicationRepository drafts)
    {
        _accounts = accounts;
        _drafts = drafts;
    }

    public async Task<AdmittedStudentsListResponse> Handle(ListAdmittedStudentsQuery request, CancellationToken cancellationToken)
    {
        var (accounts, total) = await _accounts.GetAdmittedStudentsPagedAsync(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            cancellationToken);

        var items = new List<AdmittedStudentRowDto>();
        foreach (var a in accounts)
        {
            string? major = null;
            var draftEntity = await _drafts.GetDraftByAccountIdAsync(a.Id, cancellationToken);
            if (draftEntity is not null)
            {
                var d = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _json);
                major = d?.Courses?.MajorSubject;
            }

            items.Add(new AdmittedStudentRowDto(
                a.Id,
                a.UniqueId,
                a.FullName,
                major,
                a.IsPaymentCompleted,
                a.Status.ToString(),
                a.PaymentCompletedOnUtc,
                a.CreatedOnUtc));
        }

        return new AdmittedStudentsListResponse(items, total, request.Page, request.PageSize);
    }
}

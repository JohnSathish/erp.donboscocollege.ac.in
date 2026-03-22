using System.Text.Json;
using ERP.Application.Admissions;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetApplicantApplicationDraft;

public sealed class GetApplicantApplicationDraftQueryHandler : IRequestHandler<GetApplicantApplicationDraftQuery, ApplicantApplicationDraftResponse>
{
    private readonly IApplicantApplicationRepository _repository;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public GetApplicantApplicationDraftQueryHandler(IApplicantApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApplicantApplicationDraftResponse> Handle(GetApplicantApplicationDraftQuery request, CancellationToken cancellationToken)
    {
        var draft = await _repository.GetDraftByAccountIdAsync(request.AccountId, cancellationToken);

        if (draft is null)
        {
            return new ApplicantApplicationDraftResponse
            {
                Payload = ApplicantApplicationDraftDto.Empty,
                UpdatedOnUtc = DateTime.MinValue
            };
        }

        var payload = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draft.Data, _serializerOptions)
                      ?? ApplicantApplicationDraftDto.Empty;

        ApplicantCourseSubjectNormalizer.Normalize(payload);
        AcademicSectionDraftSync.HydrateFromLegacy(payload.Academics);

        // Debug: Log the totalMarks value being loaded
        System.Diagnostics.Debug.WriteLine($"Loading draft - TotalMarks: '{payload.Academics.BoardExamination.TotalMarks}'");
        System.Diagnostics.Debug.WriteLine($"Loading draft - JSON contains totalMarks: {draft.Data.Contains("totalMarks", StringComparison.OrdinalIgnoreCase)}");

        return new ApplicantApplicationDraftResponse
        {
            Payload = payload,
            UpdatedOnUtc = draft.UpdatedOnUtc
        };
    }
}






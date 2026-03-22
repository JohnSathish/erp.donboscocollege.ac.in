using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetApplicationDocuments;

public sealed class GetApplicationDocumentsQueryHandler : IRequestHandler<GetApplicationDocumentsQuery, ApplicationDocumentsDto?>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public GetApplicationDocumentsQueryHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository)
    {
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
    }

    public async Task<ApplicationDocumentsDto?> Handle(GetApplicationDocumentsQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);

        if (account is null)
        {
            return null;
        }

        var documents = new List<DocumentDto>();

        if (account.IsApplicationSubmitted)
        {
            var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(request.AccountId, cancellationToken);
            if (draftEntity is not null)
            {
                var draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _serializerOptions)
                           ?? ApplicantApplicationDraftDto.Empty;

                var uploads = draft.Uploads ?? new UploadSection();
                var verificationStatuses = draft.DocumentVerificationStatus ?? new Dictionary<string, DTOs.DocumentVerificationStatusDto>();

                // Helper method to get verification status
                DocumentVerificationStatusDto? GetVerificationStatus(string documentType)
                {
                    if (verificationStatuses.TryGetValue(documentType, out var status))
                    {
                        return new DocumentVerificationStatusDto(status.IsVerified, status.VerifiedOnUtc, status.VerifiedBy, status.Remarks);
                    }
                    return null;
                }

                // Class X Marksheet
                if (uploads.StdXMarksheet != null && !string.IsNullOrWhiteSpace(uploads.StdXMarksheet.Data))
                {
                    var dataBytes = Convert.FromBase64String(uploads.StdXMarksheet.Data);
                    documents.Add(new DocumentDto(
                        "StdXMarksheet",
                        uploads.StdXMarksheet.FileName,
                        uploads.StdXMarksheet.ContentType,
                        dataBytes.Length,
                        true,
                        GetVerificationStatus("StdXMarksheet")));
                }
                else
                {
                    documents.Add(new DocumentDto("StdXMarksheet", "", "", 0, false, GetVerificationStatus("StdXMarksheet")));
                }

                // Class XII Marksheet
                if (uploads.StdXIIMarksheet != null && !string.IsNullOrWhiteSpace(uploads.StdXIIMarksheet.Data))
                {
                    var dataBytes = Convert.FromBase64String(uploads.StdXIIMarksheet.Data);
                    documents.Add(new DocumentDto(
                        "StdXIIMarksheet",
                        uploads.StdXIIMarksheet.FileName,
                        uploads.StdXIIMarksheet.ContentType,
                        dataBytes.Length,
                        true,
                        GetVerificationStatus("StdXIIMarksheet")));
                }
                else
                {
                    documents.Add(new DocumentDto("StdXIIMarksheet", "", "", 0, false, GetVerificationStatus("StdXIIMarksheet")));
                }

                // CUET Marksheet
                if (uploads.CuetMarksheet != null && !string.IsNullOrWhiteSpace(uploads.CuetMarksheet.Data))
                {
                    var dataBytes = Convert.FromBase64String(uploads.CuetMarksheet.Data);
                    documents.Add(new DocumentDto(
                        "CuetMarksheet",
                        uploads.CuetMarksheet.FileName,
                        uploads.CuetMarksheet.ContentType,
                        dataBytes.Length,
                        true,
                        GetVerificationStatus("CuetMarksheet")));
                }
                else
                {
                    documents.Add(new DocumentDto("CuetMarksheet", "", "", 0, false, GetVerificationStatus("CuetMarksheet")));
                }

                // Differently Abled Proof
                if (uploads.DifferentlyAbledProof != null && !string.IsNullOrWhiteSpace(uploads.DifferentlyAbledProof.Data))
                {
                    var dataBytes = Convert.FromBase64String(uploads.DifferentlyAbledProof.Data);
                    documents.Add(new DocumentDto(
                        "DifferentlyAbledProof",
                        uploads.DifferentlyAbledProof.FileName,
                        uploads.DifferentlyAbledProof.ContentType,
                        dataBytes.Length,
                        true,
                        GetVerificationStatus("DifferentlyAbledProof")));
                }
                else
                {
                    documents.Add(new DocumentDto("DifferentlyAbledProof", "", "", 0, false, GetVerificationStatus("DifferentlyAbledProof")));
                }

                // Economically Weaker Proof
                if (uploads.EconomicallyWeakerProof != null && !string.IsNullOrWhiteSpace(uploads.EconomicallyWeakerProof.Data))
                {
                    var dataBytes = Convert.FromBase64String(uploads.EconomicallyWeakerProof.Data);
                    documents.Add(new DocumentDto(
                        "EconomicallyWeakerProof",
                        uploads.EconomicallyWeakerProof.FileName,
                        uploads.EconomicallyWeakerProof.ContentType,
                        dataBytes.Length,
                        true,
                        GetVerificationStatus("EconomicallyWeakerProof")));
                }
                else
                {
                    documents.Add(new DocumentDto("EconomicallyWeakerProof", "", "", 0, false, GetVerificationStatus("EconomicallyWeakerProof")));
                }
            }
        }

        return new ApplicationDocumentsDto(
            account.Id,
            account.UniqueId,
            account.FullName,
            documents);
    }
}


using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Queries.DownloadDocument;

public sealed class DownloadDocumentQueryHandler : IRequestHandler<DownloadDocumentQuery, DocumentDownloadDto?>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public DownloadDocumentQueryHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository)
    {
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
    }

    public async Task<DocumentDownloadDto?> Handle(DownloadDocumentQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);

        if (account is null || !account.IsApplicationSubmitted)
        {
            return null;
        }

        var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(request.AccountId, cancellationToken);
        if (draftEntity is null)
        {
            return null;
        }

        var draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _serializerOptions)
                   ?? ApplicantApplicationDraftDto.Empty;

        var uploads = draft.Uploads ?? new UploadSection();
        FileAttachmentDto? attachment = null;

        attachment = request.DocumentType switch
        {
            "StdXMarksheet" => uploads.StdXMarksheet,
            "StdXIIMarksheet" => uploads.StdXIIMarksheet,
            "CuetMarksheet" => uploads.CuetMarksheet,
            "DifferentlyAbledProof" => uploads.DifferentlyAbledProof,
            "EconomicallyWeakerProof" => uploads.EconomicallyWeakerProof,
            _ => null
        };

        if (attachment is null || string.IsNullOrWhiteSpace(attachment.Data))
        {
            return null;
        }

        try
        {
            var content = Convert.FromBase64String(attachment.Data);
            return new DocumentDownloadDto(
                content,
                attachment.FileName,
                attachment.ContentType);
        }
        catch
        {
            return null;
        }
    }
}












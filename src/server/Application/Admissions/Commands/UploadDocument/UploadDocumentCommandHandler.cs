using System.Text.Json;
using ERP.Application.Admissions.Commands.SaveApplicantApplicationDraft;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.UploadDocument;

public sealed class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, UploadDocumentResult>
{
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IMediator _mediator;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
    private static readonly string[] AllowedContentTypes = { "application/pdf", "image/jpeg", "image/png", "image/jpg" };
    private static readonly string[] AllowedDocumentTypes = 
    { 
        "StdXMarksheet", 
        "StdXIIMarksheet", 
        "CuetMarksheet", 
        "DifferentlyAbledProof", 
        "EconomicallyWeakerProof" 
    };

    public UploadDocumentCommandHandler(
        IApplicantApplicationRepository applicationRepository,
        IApplicantAccountRepository accountRepository,
        IMediator mediator)
    {
        _applicationRepository = applicationRepository;
        _accountRepository = accountRepository;
        _mediator = mediator;
    }

    public async Task<UploadDocumentResult> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        // Validate document type
        if (!AllowedDocumentTypes.Contains(request.DocumentType, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                $"Invalid document type. Allowed types: {string.Join(", ", AllowedDocumentTypes)}",
                nameof(request.DocumentType));
        }

        // Validate file size
        if (request.FileData.Length > MaxFileSizeBytes)
        {
            throw new ArgumentException(
                $"File size exceeds maximum allowed size of {MaxFileSizeBytes / (1024 * 1024)} MB",
                nameof(request.FileData));
        }

        // Validate content type
        if (!AllowedContentTypes.Contains(request.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                $"Invalid content type. Allowed types: {string.Join(", ", AllowedContentTypes)}",
                nameof(request.ContentType));
        }

        // Verify account exists
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account is null)
        {
            throw new InvalidOperationException("Applicant account not found.");
        }

        // Load existing draft or create new one
        var existingDraft = await _applicationRepository.GetDraftByAccountIdAsync(request.AccountId, cancellationToken);
        var draft = existingDraft is not null
            ? JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(existingDraft.Data, _serializerOptions)
              ?? ApplicantApplicationDraftDto.Empty
            : ApplicantApplicationDraftDto.Empty;

        // Ensure uploads section exists
        draft.Uploads ??= new UploadSection();

        // Create file attachment
        var fileAttachment = new FileAttachmentDto
        {
            FileName = request.FileName,
            ContentType = request.ContentType,
            Data = Convert.ToBase64String(request.FileData)
        };

        // Update the specific document field based on document type
        var documentTypeLower = request.DocumentType.ToLowerInvariant();
        switch (documentTypeLower)
        {
            case "stdxmarksheet":
                draft.Uploads.StdXMarksheet = fileAttachment;
                break;
            case "stdxiimarksheet":
                draft.Uploads.StdXIIMarksheet = fileAttachment;
                break;
            case "cuetmarksheet":
                draft.Uploads.CuetMarksheet = fileAttachment;
                break;
            case "differentlyabledproof":
                draft.Uploads.DifferentlyAbledProof = fileAttachment;
                break;
            case "economicallyweakerproof":
                draft.Uploads.EconomicallyWeakerProof = fileAttachment;
                break;
            default:
                throw new ArgumentException($"Unsupported document type: {request.DocumentType}", nameof(request.DocumentType));
        }

        // Save the updated draft
        var saveResult = await _mediator.Send(
            new SaveApplicantApplicationDraftCommand(request.AccountId, draft),
            cancellationToken);

        return new UploadDocumentResult(
            request.DocumentType,
            request.FileName,
            request.FileData.Length,
            saveResult.UpdatedOnUtc);
    }
}


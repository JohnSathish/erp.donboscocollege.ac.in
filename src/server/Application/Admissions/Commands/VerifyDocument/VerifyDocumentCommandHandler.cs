using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using System.Text.Json;

namespace ERP.Application.Admissions.Commands.VerifyDocument;

public sealed class VerifyDocumentCommandHandler : IRequestHandler<VerifyDocumentCommand, bool>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public VerifyDocumentCommandHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository)
    {
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
    }

    public async Task<bool> Handle(VerifyDocumentCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account is null)
        {
            return false;
        }

        // Get the application draft
        var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(request.AccountId, cancellationToken);
        if (draftEntity is null)
        {
            return false;
        }

        // Deserialize the application draft
        var draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _jsonOptions);
        if (draft is null)
        {
            return false;
        }

        // Initialize document verification status if it doesn't exist
        if (draft.DocumentVerificationStatus is null)
        {
            draft.DocumentVerificationStatus = new Dictionary<string, DocumentVerificationStatusDto>();
        }

        // Update or add verification status for the document
        draft.DocumentVerificationStatus[request.DocumentType] = new DocumentVerificationStatusDto
        {
            IsVerified = request.IsVerified,
            VerifiedOnUtc = DateTime.UtcNow,
            VerifiedBy = request.VerifiedBy,
            Remarks = request.Remarks
        };

        // Serialize back to JSON
        var updatedJson = JsonSerializer.Serialize(draft, _jsonOptions);
        draftEntity.Update(updatedJson, DateTime.UtcNow);

        await _applicationRepository.UpsertDraftAsync(draftEntity, cancellationToken);

        return true;
    }
}


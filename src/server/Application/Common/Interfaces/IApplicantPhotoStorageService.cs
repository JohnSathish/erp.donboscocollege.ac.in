using ERP.Application.Common.DTOs;

namespace ERP.Application.Common.Interfaces;

public interface IApplicantPhotoStorageService
{
    Task<string> SaveProfilePhotoAsync(
        Guid accountId,
        UploadedFileDto photo,
        CancellationToken cancellationToken = default);
}





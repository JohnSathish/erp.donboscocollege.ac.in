using System.IO;
using ERP.Application.Common.DTOs;
using ERP.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Admissions;

public class ApplicantPhotoStorageService : IApplicantPhotoStorageService
{
    private const string UploadFolder = "uploads";
    private const string ApplicantsFolder = "applicants";

    private readonly IHostEnvironment _environment;
    private readonly ILogger<ApplicantPhotoStorageService> _logger;

    public ApplicantPhotoStorageService(
        IHostEnvironment environment,
        ILogger<ApplicantPhotoStorageService> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async Task<string> SaveProfilePhotoAsync(
        Guid accountId,
        UploadedFileDto photo,
        CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(photo.FileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            extension = ".jpg";
        }

        var uploadsRoot = ResolveUploadsRoot();
        var directory = Path.Combine(uploadsRoot, UploadFolder, ApplicantsFolder);
        Directory.CreateDirectory(directory);

        var fileName = $"{accountId}{extension}";
        var filePath = Path.Combine(directory, fileName);

        await File.WriteAllBytesAsync(filePath, photo.Content, cancellationToken);
        _logger.LogInformation("Stored applicant profile photo at {FilePath}", filePath);

        return $"/{UploadFolder}/{ApplicantsFolder}/{fileName}";
    }

    private string ResolveUploadsRoot()
    {
        var webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");
        Directory.CreateDirectory(webRoot);
        return webRoot;
    }
}

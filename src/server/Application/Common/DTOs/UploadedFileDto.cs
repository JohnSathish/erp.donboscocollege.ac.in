namespace ERP.Application.Common.DTOs;

public sealed record UploadedFileDto(
    string FileName,
    string ContentType,
    byte[] Content);





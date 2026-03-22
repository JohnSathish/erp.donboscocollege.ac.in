namespace ERP.Application.Admissions.DTOs;

public sealed record ApplicantApplicationPdfResult(
    string FileName,
    string ContentType,
    byte[] Content);






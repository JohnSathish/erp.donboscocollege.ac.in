namespace ERP.Application.Admissions.Options;

/// <summary>College header/footer text and optional logo path for application fee receipts (PDF).</summary>
public sealed class ReceiptBrandingOptions
{
    public string CollegeName { get; set; } = "DON BOSCO COLLEGE TURA";

    public string AddressLine1 { get; set; } = string.Empty;

    public string AddressLine2 { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;

    /// <summary>File path relative to host content root (e.g. <c>Logo.png</c> next to the API project).</summary>
    public string? LogoRelativePath { get; set; }
}

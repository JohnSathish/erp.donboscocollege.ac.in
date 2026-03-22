namespace ERP.Infrastructure.Notifications;

public class SmsSettings
{
    public bool Enabled { get; set; } = true;

    public string BaseUrl { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Sender { get; set; } = string.Empty;

    public string Priority { get; set; } = "1";

    public string LoginUrl { get; set; } = string.Empty;

    /// <summary>
    /// DLT Template ID (registered template ID from TRAI DLT portal)
    /// </summary>
    public string? DltTemplateId { get; set; }

    /// <summary>
    /// Registered template content (must match exactly with DLT registered template)
    /// </summary>
    public string? TemplateContent { get; set; }

    /// <summary>
    /// CTA (Call To Action) ID for DLT registered URL
    /// </summary>
    public string? CtaId { get; set; }
}


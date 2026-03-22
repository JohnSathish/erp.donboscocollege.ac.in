namespace ERP.Infrastructure.Payments;

/// <summary>
/// Razorpay API keys. In the Razorpay Dashboard (Settings → Payment Methods), enable UPI,
/// UPI Intent, and UPI QR for the account linked to <see cref="KeyId"/> so web checkout can offer
/// QR on desktop and app intent on mobile.
/// </summary>
public class RazorpaySettings
{
    public string KeyId { get; set; } = string.Empty;
    public string KeySecret { get; set; } = string.Empty;
    public bool TestMode { get; set; } = true;
    public decimal ApplicationFeeAmount { get; set; } = 600.00m;
}



















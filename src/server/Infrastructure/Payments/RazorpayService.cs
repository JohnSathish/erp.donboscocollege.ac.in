using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ERP.Infrastructure.Payments;

public interface IRazorpayService
{
    Task<RazorpayOrderResponse> CreateOrderAsync(decimal amount, string currency, string receipt, Dictionary<string, string>? notes = null, CancellationToken cancellationToken = default);
    Task<bool> VerifyPaymentAsync(string orderId, string paymentId, string signature, CancellationToken cancellationToken = default);
}

public class RazorpayService : IRazorpayService
{
    private readonly RazorpaySettings _settings;
    private readonly HttpClient _httpClient;
    private readonly ILogger<RazorpayService> _logger;
    private const string RazorpayApiBaseUrl = "https://api.razorpay.com/v1";

    public RazorpayService(
        IOptions<RazorpaySettings> settings,
        HttpClient httpClient,
        ILogger<RazorpayService> logger)
    {
        _settings = settings.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<RazorpayOrderResponse> CreateOrderAsync(
        decimal amount,
        string currency,
        string receipt,
        Dictionary<string, string>? notes = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_settings.KeyId) || string.IsNullOrWhiteSpace(_settings.KeySecret))
        {
            throw new InvalidOperationException("Razorpay credentials are not configured.");
        }

        var requestBody = new
        {
            amount = (int)(amount * 100), // Convert to paise
            currency = currency,
            receipt = receipt,
            notes = notes ?? new Dictionary<string, string>()
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            _logger.LogInformation("Creating Razorpay order: Amount={Amount}, Currency={Currency}, Receipt={Receipt}", amount, currency, receipt);
            
            // Create request message with authorization header
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.KeyId}:{_settings.KeySecret}"));
            var request = new HttpRequestMessage(HttpMethod.Post, "orders")
            {
                Content = content
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authValue);
            
            var response = await _httpClient.SendAsync(request, cancellationToken);
            
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Razorpay API error: Status={StatusCode}, Response={Response}", response.StatusCode, responseContent);
                throw new InvalidOperationException($"Razorpay API error: {response.StatusCode} - {responseContent}");
            }

            var orderResponse = JsonSerializer.Deserialize<RazorpayOrderResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (orderResponse == null)
            {
                _logger.LogError("Failed to parse Razorpay order response: {Response}", responseContent);
                throw new InvalidOperationException($"Failed to parse Razorpay order response: {responseContent}");
            }

            _logger.LogInformation("Razorpay order created successfully: OrderId={OrderId}", orderResponse.Id);
            return orderResponse;
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP error while creating Razorpay order");
            throw new InvalidOperationException($"Failed to communicate with Razorpay API: {httpEx.Message}", httpEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Razorpay order");
            throw;
        }
    }

    public async Task<bool> VerifyPaymentAsync(
        string orderId,
        string paymentId,
        string signature,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_settings.KeySecret))
        {
            throw new InvalidOperationException("Razorpay key secret is not configured.");
        }

        try
        {
            var payload = $"{orderId}|{paymentId}";
            var secretBytes = Encoding.UTF8.GetBytes(_settings.KeySecret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using var hmac = new System.Security.Cryptography.HMACSHA256(secretBytes);
            var hashBytes = hmac.ComputeHash(payloadBytes);
            var computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            return computedSignature.Equals(signature, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to verify Razorpay payment signature");
            return false;
        }
    }
}

public class RazorpayOrderResponse
{
    public string Id { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Receipt { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int CreatedAt { get; set; }
}


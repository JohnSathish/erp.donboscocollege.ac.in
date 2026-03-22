using System.Collections.Generic;
using System.Linq;
using ERP.Application.Admissions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ERP.Infrastructure.Notifications;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/test")]
public class EmailTestController : ControllerBase
{
    private readonly EmailSettings _emailSettings;
    private readonly SmsSettings _smsSettings;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<EmailTestController> _logger;

    public EmailTestController(
        IOptions<EmailSettings> emailOptions,
        IOptions<SmsSettings> smsOptions,
        IApplicantNotificationService notificationService,
        ILogger<EmailTestController> logger)
    {
        _emailSettings = emailOptions.Value;
        _smsSettings = smsOptions.Value;
        _notificationService = notificationService;
        _logger = logger;
    }

    [HttpPost("email")]
    public async Task<ActionResult> TestEmail([FromBody] TestEmailRequest request)
    {
        try
        {
            var testEmail = request.ToEmail ?? _emailSettings.FromAddress;
            
            _logger.LogInformation("=== EMAIL TEST STARTED ===");
            _logger.LogInformation("SMTP Host: {Host}", _emailSettings.SmtpHost);
            _logger.LogInformation("SMTP Port: {Port}", _emailSettings.SmtpPort);
            _logger.LogInformation("Use SSL: {UseSsl}", _emailSettings.UseSsl);
            _logger.LogInformation("Username: {Username}", _emailSettings.Username);
            _logger.LogInformation("From Address: {FromAddress}", _emailSettings.FromAddress);
            _logger.LogInformation("Password Length: {Length}", _emailSettings.Password?.Length ?? 0);
            _logger.LogInformation("Test Email To: {TestEmail}", testEmail);

            // Use the notification service to send a test registration email
            await _notificationService.SendRegistrationNotificationsAsync(
                "Test User",
                "TEST001",
                testEmail,
                "1234567890",
                "TestPassword123",
                CancellationToken.None);

            _logger.LogInformation("=== EMAIL TEST SUCCESS ===");

            return Ok(new
            {
                success = true,
                message = "Test email sent successfully! Check your inbox.",
                details = new
                {
                    smtpHost = _emailSettings.SmtpHost,
                    smtpPort = _emailSettings.SmtpPort,
                    useSsl = _emailSettings.UseSsl,
                    username = _emailSettings.Username,
                    fromAddress = _emailSettings.FromAddress,
                    testEmailTo = testEmail
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "=== EMAIL TEST FAILED ===");
            
            var errorDetails = new
            {
                errorMessage = ex.Message,
                errorType = ex.GetType().Name,
                stackTrace = ex.StackTrace,
                innerException = ex.InnerException != null ? new
                {
                    message = ex.InnerException.Message,
                    type = ex.InnerException.GetType().Name
                } : null
            };

            return StatusCode(500, new
            {
                success = false,
                message = "Failed to send test email",
                error = ex.Message,
                errorType = ex.GetType().Name,
                details = new
                {
                    smtpHost = _emailSettings.SmtpHost,
                    smtpPort = _emailSettings.SmtpPort,
                    useSsl = _emailSettings.UseSsl,
                    username = _emailSettings.Username,
                    fromAddress = _emailSettings.FromAddress,
                    passwordConfigured = !string.IsNullOrWhiteSpace(_emailSettings.Password),
                    passwordLength = _emailSettings.Password?.Length ?? 0,
                    errorDetails
                }
            });
        }
    }

    [HttpGet("email-config")]
    public ActionResult GetEmailConfig()
    {
        return Ok(new
        {
            enabled = _emailSettings.Enabled,
            smtpHost = _emailSettings.SmtpHost,
            smtpPort = _emailSettings.SmtpPort,
            useSsl = _emailSettings.UseSsl,
            username = _emailSettings.Username,
            fromAddress = _emailSettings.FromAddress,
            fromName = _emailSettings.FromName,
            passwordConfigured = !string.IsNullOrWhiteSpace(_emailSettings.Password),
            passwordLength = _emailSettings.Password?.Length ?? 0
        });
    }

    [HttpPost("sms")]
    public async Task<ActionResult> TestSms([FromBody] TestSmsRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.MobileNumber))
            {
                return BadRequest(new { message = "Mobile number is required" });
            }

            _logger.LogInformation("=== SMS TEST STARTED ===");
            _logger.LogInformation("Base URL: {BaseUrl}", _smsSettings.BaseUrl);
            _logger.LogInformation("Username: {Username}", _smsSettings.Username);
            _logger.LogInformation("Sender: {Sender}", _smsSettings.Sender);
            _logger.LogInformation("Priority: {Priority}", _smsSettings.Priority);
            _logger.LogInformation("Test Mobile Number: {MobileNumber}", request.MobileNumber);

            // Build the SMS URI to log it
            var testMessage = "Test SMS from ERP System";
            var queryParams = new Dictionary<string, string>
            {
                ["uname"] = _smsSettings.Username,
                ["pass"] = _smsSettings.Password,
                ["send"] = _smsSettings.Sender,
                ["dest"] = request.MobileNumber,
                ["msg"] = testMessage,
                ["priority"] = _smsSettings.Priority,
                ["schtm"] = string.Empty
            };
            
            var builder = new UriBuilder(_smsSettings.BaseUrl);
            var query = string.Join("&", queryParams
                .Where(kvp => kvp.Value is not null)
                .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value!)}"));
            builder.Query = query;
            var testUri = builder.Uri.ToString().Replace(_smsSettings.Password, "****");
            
            _logger.LogInformation("Test SMS URI (password hidden): {TestUri}", testUri);

            // Use the notification service to send a test registration SMS
            await _notificationService.SendRegistrationNotificationsAsync(
                "Test User",
                "TEST001",
                "test@example.com",
                request.MobileNumber,
                "TestPassword123",
                CancellationToken.None);

            _logger.LogInformation("=== SMS TEST SUCCESS ===");

            return Ok(new
            {
                success = true,
                message = "Test SMS sent successfully! Check your phone. If not received, check backend logs for provider response.",
                details = new
                {
                    baseUrl = _smsSettings.BaseUrl,
                    username = _smsSettings.Username,
                    sender = _smsSettings.Sender,
                    priority = _smsSettings.Priority,
                    testMobileNumber = request.MobileNumber,
                    note = "Check backend logs for actual SMS provider response"
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "=== SMS TEST FAILED ===");
            
            var errorDetails = new
            {
                errorMessage = ex.Message,
                errorType = ex.GetType().Name,
                stackTrace = ex.StackTrace,
                innerException = ex.InnerException != null ? new
                {
                    message = ex.InnerException.Message,
                    type = ex.InnerException.GetType().Name
                } : null
            };

            return StatusCode(500, new
            {
                success = false,
                message = "Failed to send test SMS",
                error = ex.Message,
                errorType = ex.GetType().Name,
                details = new
                {
                    baseUrl = _smsSettings.BaseUrl,
                    username = _smsSettings.Username,
                    sender = _smsSettings.Sender,
                    priority = _smsSettings.Priority,
                    enabled = _smsSettings.Enabled,
                    errorDetails
                }
            });
        }
    }

    [HttpGet("sms-config")]
    public ActionResult GetSmsConfig()
    {
        return Ok(new
        {
            enabled = _smsSettings.Enabled,
            baseUrl = _smsSettings.BaseUrl,
            username = _smsSettings.Username,
            sender = _smsSettings.Sender,
            priority = _smsSettings.Priority,
            loginUrl = _smsSettings.LoginUrl,
            dltTemplateId = _smsSettings.DltTemplateId,
            templateContent = _smsSettings.TemplateContent,
            ctaId = _smsSettings.CtaId,
            passwordConfigured = !string.IsNullOrWhiteSpace(_smsSettings.Password),
            passwordLength = _smsSettings.Password?.Length ?? 0
        });
    }
}

public record TestEmailRequest(string? ToEmail);
public record TestSmsRequest(string MobileNumber);


using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ERP.Api.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace ERP.Api.IntegrationTests.Admissions;

public class ApplicantPortalTests(AdmissionsApiFactory factory) : IClassFixture<AdmissionsApiFactory>
{
    private readonly AdmissionsApiFactory _factory = factory;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task GetDashboard_ReturnsProfileDocumentsAndNotifications()
    {
        var password = "Temp@Portal1!";
        var account = await _factory.CreateApplicantAccountAsync(
            "APP-6001",
            "Portal Test",
            "portal.test@example.com",
            "9000000100",
            "SHIFT - I",
            password,
            mustChangePassword: false);

        var client = _factory.CreateClient();
        var login = await LoginAsync(client, account.Email, password);

        using var message = new HttpRequestMessage(HttpMethod.Get, "api/applicant-portal/me");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", login.Token);

        var response = await client.SendAsync(message);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dashboard = await response.Content.ReadFromJsonAsync<ApplicantDashboardResponse>(JsonOptions);
        dashboard.Should().NotBeNull();
        dashboard!.Profile.FullName.Should().Be(account.FullName);
        dashboard.Profile.UniqueId.Should().Be(account.UniqueId);
        dashboard.Documents.Should().NotBeEmpty();
        dashboard.Notifications.Should().NotBeEmpty();
    }

    private static async Task<LoginResponse> LoginAsync(HttpClient client, string username, string password)
    {
        var response = await client.PostAsJsonAsync("api/auth/applicants/login", new LoginRequest(username, password));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<LoginResponse>(JsonOptions))!;
    }

    private sealed record LoginRequest(string Username, string Password);

    private sealed record LoginResponse(
        string Token,
        DateTime ExpiresAtUtc,
        string RefreshToken,
        DateTime RefreshTokenExpiresAtUtc,
        string UniqueId,
        string Email,
        string FullName,
        bool MustChangePassword);

    private sealed record ApplicantDashboardResponse(
        ApplicantProfileResponse Profile,
        IReadOnlyCollection<ApplicantDocumentResponse> Documents,
        IReadOnlyCollection<ApplicantNotificationResponse> Notifications);

    private sealed record ApplicantProfileResponse(
        Guid AccountId,
        string UniqueId,
        string FullName,
        DateOnly DateOfBirth,
        string Gender,
        string Email,
        string MobileNumber,
        string Shift,
        string? PhotoUrl,
        DateTime CreatedOnUtc);

    private sealed record ApplicantDocumentResponse(
        string Name,
        string Status,
        string Description,
        bool IsComplete);

    private sealed record ApplicantNotificationResponse(
        string Title,
        string Message,
        DateTime CreatedOnUtc);
}







